using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public struct PlayerCharacterInputs
{
    public float MoveAxisForward;
    public float MoveAxisRight;
    public bool JumpDown;
    
    public Quaternion CameraRotation;
    public float MouseLookAxisUp;
    public float MouseLookAxisRight;
}

public class Player : MonoBehaviour
{
    public CharacterController Character;
    public CharacterCamera CharacterCamera;
    public Image crosshair;
    
    [Header("Sounds")]
    public FMODUnity.EventReference footstepSoundEvent;
    public float stepDelay = 0.5f;

    private PlayerCharacterInputs _characterInputs = new PlayerCharacterInputs();
    private float _timeOfLastStep;

    public void OnMoveVertical(InputValue value)
    {
        this._characterInputs.MoveAxisForward = value.Get<float>();
    }
    
    public void OnMoveHorizontal(InputValue value)
    {
        this._characterInputs.MoveAxisRight = value.Get<float>();
    }

    public void OnJump(InputValue value)
    {
        var down = value.Get<float>();
        if (down > 0f)
        {
            this._characterInputs.JumpDown = true;
        }
        else
        {
            this._characterInputs.JumpDown = false;
        }
    }
    
    public void OnLookVertical(InputValue value)
    {
        this._characterInputs.MouseLookAxisUp = value.Get<float>();
    }
    
    public void OnLookHorizontal(InputValue value)
    {
        this._characterInputs.MouseLookAxisRight = value.Get<float>();
    }

    public void OnUse()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 3f))
        {
            IInteractable x = hit.transform.GetComponentInParent<IInteractable>();
            if (x != null)
            {
                this.Deactivate();
                x.Interact();
            }
        }
    }

    public void Activate()
    {
        this.CharacterCamera.Camera.enabled = true;
        this.GetComponent<PlayerInput>().enabled = true;
        this.crosshair.enabled = true;
        
        var renderers = Character.GetComponentsInChildren<MeshRenderer>();
        foreach (var r in renderers)
        {
            r.enabled = true;
        }
    }

    public void Deactivate()
    {
        this.CharacterCamera.Camera.enabled = false;
        this.GetComponent<PlayerInput>().enabled = false;
        this.crosshair.enabled = false;
        
        var renderers = Character.GetComponentsInChildren<MeshRenderer>();
        foreach (var r in renderers)
        {
            r.enabled = false;
        }
    }
    
    private void HandleCameraInput()
    {
        // Create the look input vector for the camera
        Vector3 lookInputVector = new Vector3(this._characterInputs.MouseLookAxisRight, this._characterInputs.MouseLookAxisUp, 0f);

        // Prevent moving the camera while the cursor isn't locked
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            lookInputVector = Vector3.zero;
        }

        // Apply inputs to the camera
        CharacterCamera.UpdateWithInput(Time.deltaTime, 0f, lookInputVector);

        // Handle toggling zoom level
        if (Input.GetMouseButtonDown(1))
        {
            CharacterCamera.TargetDistance = (CharacterCamera.TargetDistance == 0f) ? CharacterCamera.DefaultDistance : 0f;
        }
    }

    private void HandleCharacterInput()
    {
        // Build the CharacterInputs struct
        _characterInputs.CameraRotation = CharacterCamera.Transform.rotation;

        // Apply inputs to character
        Character.SetInputs(ref _characterInputs);
    }

    private void PlayFootstepSound()
    {
        FMOD.Studio.EventInstance eInstance = FMODUnity.RuntimeManager.CreateInstance(this.footstepSoundEvent);

        // play sound
        eInstance.start();
        eInstance.release();
    }

    private void Start()
    {
        this.crosshair.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;

        // Tell camera to follow transform
        CharacterCamera.SetFollowTransform(Character.CameraFollowPoint);

        // Ignore the character's collider(s) for camera obstruction checks
        CharacterCamera.IgnoredColliders.Clear();
        CharacterCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());

        this._timeOfLastStep = Time.time;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        HandleCharacterInput();

        if (Time.time - this._timeOfLastStep > this.stepDelay)
        {
            if (this._characterInputs.MoveAxisForward != 0f || this._characterInputs.MoveAxisRight != 0f)
            {
                this.PlayFootstepSound();
                this._timeOfLastStep = Time.time;
            }
        }
    }
    
    private void LateUpdate()
    {
        // Handle rotating the camera along with physics movers
        if (CharacterCamera.RotateWithPhysicsMover && Character.motor.AttachedRigidbody != null)
        {
            CharacterCamera.PlanarDirection = Character.motor.AttachedRigidbody.GetComponent<PhysicsMover>().RotationDeltaFromInterpolation * CharacterCamera.PlanarDirection;
            CharacterCamera.PlanarDirection = Vector3.ProjectOnPlane(CharacterCamera.PlanarDirection, Character.motor.CharacterUp).normalized;
        }

        HandleCameraInput();
    }
}
