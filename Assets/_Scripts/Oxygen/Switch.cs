using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IInteractable
{
    [Header("External References")] 
    public Player player;
    
    [Header("Internal References")] 
    public Transform pivot;
    
    [Header("Stats")]
    public float maxAngle = 25f;
    
    [System.NonSerialized] public bool on = false;
    
    public void Interact()
    {
        this.player.Activate();
        
        this.on = !this.on;
        if (this.on)
        {
            this.pivot.localRotation = Quaternion.Euler(0f, 0f, this.maxAngle);
        }
        else
        {
            this.pivot.localRotation = Quaternion.Euler(0f, 0f, -this.maxAngle);
        }
    }
    
    void Start()
    {
        if (this.on)
        {
            this.pivot.localRotation = Quaternion.Euler(0f, 0f, this.maxAngle);
        }
        else
        {
            this.pivot.localRotation = Quaternion.Euler(0f, 0f, -this.maxAngle);
        }
    }

    void Update()
    {
        
    }
}
