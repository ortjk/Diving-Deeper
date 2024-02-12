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
    
    [Header("Stats")] 
    public float maxAngle = 90f;
    public float targetPressure = 1f;
    public float percentMargin = 0.2f;
    
    [System.NonSerialized] public float internalPressure = 0f;
    [System.NonSerialized] public float externalPressure = 0f;
    [System.NonSerialized] public bool inPressure = false;
    
    private const float p = 9.81f * 1026f;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        this.externalPressure = this.timer.currentDepth * p;
        this.internalPressure = this.externalPressure * this.valve.fractionOpen;

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
    }
}
