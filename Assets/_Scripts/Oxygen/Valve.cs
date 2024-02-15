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

    [System.NonSerialized] public float turnDelta = 0f;

    public void OnUse(InputValue value)
    {
        this.Deactivate();
        this.player.Activate();
    }
    
    public void OnTurn(InputValue value)
    {
        this.turnDelta = value.Get<float>();
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

    /*
    private float GetXRotationBetweenZeroAnd360(Quaternion r)
    {
        float xRotation = r.eulerAngles.x;
        
        // bottom two quadrants of circle
        if (r.eulerAngles.y > 1f)
        {
            // quadrant 3
            if (xRotation <= 90f)
            {
                return 180f - xRotation;
            }
            
            // quadrant 4
            if (xRotation >= 270f)
            {
                return 180f + 360f - xRotation;
            }
        }

        // reading is accurate for top two quadrants of circle
        return xRotation;
    }
    */
    
    void Start()
    {
        this.pivot.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void Update()
    {
        float dt = Time.deltaTime;
        
        this.pivot.Rotate(this.turnDelta * this.changingSpeed * dt, 0f, 0f);
        
        /*
        float realRotation = this.GetXRotationBetweenZeroAnd360(this.pivot.localRotation);

        if (realRotation > this.maxAngle + 1f)
        {
            this.pivot.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (realRotation > this.maxAngle)
        {
            this.pivot.localRotation = Quaternion.Euler(this.maxAngle, 0f, 0f);
        }
        */
        
        
    }
}
