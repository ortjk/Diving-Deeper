using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Valve : MonoBehaviour, IInteractable
{
    [Header("External References")] 
    public Player player;
    
    [Header("Internal References")] 
    public Camera valveCamera;
    public Transform pivot;

    [Header("Stats")] 
    public float changingSpeed = 10f;
    
    [Header("Sounds")]
    public FMODUnity.EventReference turnSoundEvent;
    
    public event IInteractable.Interacted OnInteract;
    public event IInteractable.Interacted OnUninteract;

    [System.NonSerialized] public float turnDelta = 0f;

    private FMOD.Studio.EventInstance _eInstance;

    public void OnUse(InputValue value)
    {
        this.Deactivate();
        this.player.Activate();
    }
    
    public void OnTurn(InputValue value)
    {
        this.turnDelta = value.Get<float>();
        this.UpdateSound();
    }
    
    public void Interact()
    {
        this.valveCamera.enabled = true;
        this.GetComponent<PlayerInput>().enabled = true;
    }

    public void Deactivate()
    {
        this.valveCamera.enabled = false;
        this.GetComponent<PlayerInput>().enabled = false;
    }
    
    private void UpdateSound()
    {
        this._eInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        this._eInstance.release();
        
        if (this.turnDelta > 0f)
        {
            this._eInstance = FMODUnity.RuntimeManager.CreateInstance(this.turnSoundEvent);
            this._eInstance.setParameterByName("Turn Direction", 0);

            this._eInstance.start();
        }
        else if (this.turnDelta < 0f)
        {
            this._eInstance = FMODUnity.RuntimeManager.CreateInstance(this.turnSoundEvent);
            this._eInstance.setParameterByName("Turn Direction", 1);

            this._eInstance.start();
        }
    }
    void Start()
    {
        this.pivot.localRotation = Quaternion.Euler(0f, 0f, 0f);
        this._eInstance = FMODUnity.RuntimeManager.CreateInstance(this.turnSoundEvent);
    }

    void Update()
    {
        float dt = Time.deltaTime;
        
        this.pivot.Rotate(0f, this.turnDelta * this.changingSpeed * dt, 0f);
    }
}
