using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxySwitch : MonoBehaviour, IInteractable
{
    [Header("External References")] 
    public Player player;
    public LevelTimer timer;
    
    [Header("Internal References")] 
    public Transform pivot;
    
    [Header("Stats")]
    public float maxAngle = 25f;
    
    [Header("Sounds")]
    public FMODUnity.EventReference turnSoundEvent;
    public event IInteractable.Interacted OnInteract;
    public event IInteractable.Interacted OnUninteract;
    
    [System.NonSerialized] public bool on = false;
    [System.NonSerialized] public bool canEmpty = false;
    
    public void Interact()
    {
        this.player.Activate();

        this.Toggle();
    }

    private void Toggle()
    {
        this.on = !this.on;
        if (this.on)
        {
            this.pivot.localRotation = Quaternion.Euler(-this.maxAngle, 0f, 0f);
        }
        else
        {
            this.pivot.localRotation = Quaternion.Euler(this.maxAngle, 0f, 0f);
        }
        
        this.PlayToggleSound();
    }

    private void PlayToggleSound()
    {
        FMOD.Studio.EventInstance eInstance = FMODUnity.RuntimeManager.CreateInstance(this.turnSoundEvent);
        eInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform.position));
        
        if (this.on)
        {
            eInstance.setParameterByName("Switch Direction", 1);
        }
        else
        {
            eInstance.setParameterByName("Switch Direction", 0);
        }

        // play sound
        eInstance.start();
        eInstance.release();
    }
    
    void Start()
    {
        this.on = !this.on;
        this.Toggle();
    }

    void Update()
    {
        if (!canEmpty || !this.on)
        {
            this.timer.oxyIncrementMultiplier = -1f;
        }
        else
        {
            this.timer.oxyIncrementMultiplier = 2f;
        }
    }
}
