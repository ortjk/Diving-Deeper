using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Valve : MonoBehaviour, IInteractable
{
    [Header("Internal References")] 
    public Camera valveCamera;
    public Transform pivot;

    [Header("Stats")] 
    public float maxAngle = 350f;
    public float changingSpeed = 10f;

    [System.NonSerialized] public float fractionOpen = 0f;

    private float _turnDelta = 0f;

    public void OnTurn(InputValue value)
    {
        this._turnDelta = value.Get<float>();
    }
    
    public void Interact()
    {
        this.valveCamera.enabled = true;
        this.GetComponent<PlayerInput>().enabled = true;
    }

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
    
    void Start()
    {
        this.pivot.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void Update()
    {
        float dt = Time.deltaTime;
        
        this.pivot.Rotate(this._turnDelta * this.changingSpeed * dt, 0f, 0f);
        float realRotation = this.GetXRotationBetweenZeroAnd360(this.pivot.localRotation);

        if (realRotation > this.maxAngle + 1f)
        {
            this.pivot.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (realRotation > this.maxAngle)
        {
            this.pivot.localRotation = Quaternion.Euler(this.maxAngle, 0f, 0f);
        }
        
        this.fractionOpen = Mathf.InverseLerp(0f, this.maxAngle, this.GetXRotationBetweenZeroAnd360(this.pivot.localRotation));
    }
}
