using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureMeter : MonoBehaviour
{
    [Header("External References")] 
    public LevelTimer timer;
    public Valve valve;
    
    [Header("Internal References")] 
    public Transform pivot;
    
    [Header("Display")]
    public float maxAngle = 90f;

    [Header("Pressure")] 
    public float buildSpeed = 0.01f;
    public float maxPressure = 10f;
    public float targetPressure = 3f;
    public float percentMargin = 0.2f;
    
    /*[System.NonSerialized]*/ public float internalPressure = 0f;
    [System.NonSerialized] public bool inPressure = false;

    void Start()
    {
        
    }
    
    void Update()
    {
        float dt = Time.deltaTime;
        
        this.internalPressure += this.timer.depthIncrementMultiplier * this.buildSpeed * dt;
        this.internalPressure += this.valve.turnDelta * 0.3f * dt;

        if (this.internalPressure > this.targetPressure)
        {
            if (this.internalPressure < this.targetPressure * (1f + this.percentMargin))
            {
                this.inPressure = true;
            }
        }
        else if (this.internalPressure < this.targetPressure)
        {
            if (this.internalPressure > this.targetPressure * (1f - this.percentMargin))
            {
                this.inPressure = true;
            }
        }
        else
        {
            this.inPressure = true;
        }

        float lVal = Mathf.InverseLerp(0f, maxPressure, this.internalPressure);
        float rotationValue = Mathf.Lerp(maxAngle, -maxAngle, lVal);
        this.pivot.localRotation = Quaternion.Euler(0f, 0f, rotationValue);
    }
}
