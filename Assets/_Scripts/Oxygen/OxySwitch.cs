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
