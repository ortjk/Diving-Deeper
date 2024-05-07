using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Scope : PartOfTutorial, IInteractable
{
    [Header("External References")]
    public Image crosshair;
    public Player player;
    
    [Header("Internal References")]
    public Camera scopeCamera;
    public MMF_Player shootFeedback;
    
    [Header("Prefabs")]
    public GameObject missilePrefab;
    
    [Header("Stats")] 
    public float shotCooldown = 2f;
    
    [Header("Viewport")]
    public float sensitivity = 0.2f;
    public float minimumX = -360f;
    public float maximumX = 360f;
    public float minimumY = -360f;
    public float maximumY = 360;
    
    public event IInteractable.Interacted OnInteract;
    public event IInteractable.Interacted OnUninteract;

    private float _xDelta = 0f;
    private float _yDelta = 0f;

    private float _rotationX = 0f;
    private float _rotationY = 0f;
    
    private Enemy _lastTargeted;

    private bool _shotValid = false;
    private float _timeOfLastShot = 0f;

    public void OnLookVertical(InputValue value)
    {
        this._yDelta = value.Get<float>();
    }

    public void OnLookHorizontal(InputValue value)
    {
        this._xDelta = value.Get<float>();
    }

    public void OnUse()
    {
        this.Deactivate();
        player.Activate();
    }

    public void OnShoot()
    {
        if (this._lastTargeted != null)
        {
            Transform t = this._lastTargeted.transform;
        
            if (this._lastTargeted.AttemptShot() && Time.time - this._timeOfLastShot >= this.shotCooldown)
            {
                this._timeOfLastShot = Time.time;
                this.ShootMissile(t);
            }
        }
    }

    public void Interact()
    {
        this.Activate();
        if (this.OnInteract != null)
        {
            this.OnInteract.Invoke();
            this.finishedTutorial = true;
        }
    }

    private void Activate()
    {
        this.scopeCamera.enabled = true;
        this.GetComponent<PlayerInput>().enabled = true;
        this.crosshair.enabled = true;
    }

    private void Deactivate()
    {
        this.scopeCamera.enabled = false;
        this.GetComponent<PlayerInput>().enabled = false;
        this.crosshair.enabled = false;
        
        if (this.OnUninteract != null)
        {
            this.OnUninteract.Invoke();
        }
    }

    private void ShootMissile(Transform target)
    {
        var m = Instantiate(this.missilePrefab, this.transform);
        m.transform.LookAt(target);
        
        // set missing transforms
        var posFeedback = this.shootFeedback.GetFeedbackOfType<MMF_Position>();
        posFeedback.AnimatePositionTarget = m;
        posFeedback.InitialPositionTransform = m.transform;
        posFeedback.DestinationPositionTransform = target.transform;
        
        // destroy missile and target after missile shot
        var destroyFeedback = this.shootFeedback.GetFeedbackOfType<MMF_Destroy>();
        destroyFeedback.TargetGameObject = m;
        destroyFeedback.ExtraTargetGameObjects.Clear();
        destroyFeedback.ExtraTargetGameObjects.Add(target.gameObject);
        
        // missile animation location and rotation
        var instantiateFeedback = this.shootFeedback.GetFeedbackOfType<MMF_InstantiateObject>();
        instantiateFeedback.TargetTransform = target;

        this.shootFeedback.PlayFeedbacks();
    }

    private void CheckTarget(float dt)
    {
        RaycastHit hit;
        Ray ray = scopeCamera.ScreenPointToRay(Input.mousePosition);
        if (this.scopeCamera.enabled)
        {
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                Enemy e = hit.transform.GetComponentInParent<Enemy>();
                if (e != null)
                {
                    e.MousedOver(dt);
                    this._lastTargeted = e;
                }
            }
            else if (this._lastTargeted != null)
            {
                this._lastTargeted.ResetTargeting();
                this._lastTargeted = null;
            }
        }
    }
    
    void Start()
    {
        base.Start();
    }

    private void UpdateRotation()
    {
        this._rotationX += this._xDelta * this.sensitivity * SettingsMenu.Sensitivity;
        this._rotationX = Mathf.Clamp(this._rotationX, this.minimumX, this.maximumX);

        this._rotationY += this._yDelta * this.sensitivity * SettingsMenu.Sensitivity;
        this._rotationY = Mathf.Clamp(this._rotationY, this.minimumY, this.maximumY);

        this.scopeCamera.transform.localEulerAngles = new Vector3(-this._rotationY, this._rotationX, 0);
    }
    
    void Update()
    {
        float dt = Time.deltaTime;
        
        this.UpdateRotation();
        this.CheckTarget(dt);
    }
}
