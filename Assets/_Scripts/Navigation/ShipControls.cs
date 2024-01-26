using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipControls : MonoBehaviour, IInteractable
{
    [Header("External References")] 
    public Camera controlsCamera;
    public Player player;
    public Transform speedStick;

    [Header("Stats")] 
    public float maxAngle = 20f;
    public float changingSpeed = 1f;
    public float maxSpeed = 10f;

    [System.NonSerialized] public float speed = 0f;

    private float _xDelta = 0f;
    private float _yDelta = 0f;

    public void OnUse()
    {
        this.Deactivate();
        player.Activate();
    }

    public void OnChangeSpeed(InputValue value)
    {
        this._yDelta = value.Get<float>();
    }
    
    public void OnChangeDirection(InputValue value)
    {
        this._xDelta = value.Get<float>();
    }
    
    public void Interact()
    {
        this.controlsCamera.enabled = true;
        this.GetComponent<PlayerInput>().enabled = true;
    }

    private void Deactivate()
    {
        this.controlsCamera.enabled = false;
        this.GetComponent<PlayerInput>().enabled = false;
    }

    private void Awake()
    {
        this.UpdateSpeed();
    }

    void Start()
    {
        
    }

    private void UpdateSpeed()
    {
        float xRotation = this.speedStick.localRotation.eulerAngles.x;
        if (xRotation > maxAngle)
        {
            xRotation -= 360f;
        }
        
        this.speed = this.maxSpeed * (xRotation + this.maxAngle) / (this.maxAngle * 2);
        Debug.Log(xRotation);
    }

    void Update()
    {
        float dt = Time.deltaTime;
        this.speedStick.Rotate(this._yDelta * this.changingSpeed * dt, 0f, 0f);
        
        float xRotation = this.speedStick.localRotation.eulerAngles.x;
        if (xRotation < this.maxAngle + 1f)
        {
            if (xRotation > this.maxAngle - 0.01f)
            {
                this.speedStick.localRotation = Quaternion.Euler(this.maxAngle - 0.01f, 0f, 0f);
            }
        }
        
        if (xRotation > this.maxAngle + 1f)
        {
            if (xRotation - 360 < -this.maxAngle + 0.01f)
            {
                this.speedStick.localRotation = Quaternion.Euler(-this.maxAngle + 0.01f, 0f, 0f);
            }
        }
        
        this.UpdateSpeed();
    }
}
