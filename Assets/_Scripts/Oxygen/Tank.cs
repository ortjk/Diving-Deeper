using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    [Header("External References")] 
    public PressureMeter meter;
    public OxySwitch oxySwitch;

    [Header("Rates")] 
    public float generationRate;

    [Header("Contents")]
    public float waterFull = 1f;
    public float oxyFull = 0.5f;

    void Start()
    {
        
    }

    void UpdateTanks(float dt)
    {
        // filling oxygen tank
        if (oxySwitch.on)
        {
            if (this.oxySwitch.canEmpty)
            {
                oxyFull -= generationRate * 1.5f * dt;

                if (this.oxyFull < 0f)
                {
                    this.oxyFull = 0f;
                    this.oxySwitch.canEmpty = false;
                }
            }
        }
        else if (waterFull > 0.01f)
        {
            waterFull -= generationRate * 4f * dt ;
            if (waterFull < 0f)
            {
                waterFull = 0f;
            }

            oxyFull += generationRate * dt;
            if (oxyFull > 1f)
            {
                oxyFull = 1f;
            }
        }
        
        // filling water tank
        if (meter.inPressure)
        {
            waterFull += generationRate * 2f * dt;
            if (waterFull > 1f)
            {
                waterFull = 1f;
            }
        }
    }
    
    void Update()
    {
        float dt = Time.deltaTime;
        
        if (this.oxyFull > 0f)
        {
            this.oxySwitch.canEmpty = true;
        }

        this.UpdateTanks(dt);
    }
}
