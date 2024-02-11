using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelTimer : MonoBehaviour
{
    [Header("Stats")]
    public float thresholdTime = 2f;
    public float depthBaseIncrementAmount = 10;
    
    [Header("UI")]
    public TMP_Text depthCorrespondingText;
    public string depthLeadingText = "";
    public string depthPostText = "";
    
    public TMP_Text oxyCorrespondingText;
    public string oxyLeadingText = "";
    public string oxyPostText = "";

    [System.NonSerialized] public float currentDepth = 4f;
    [System.NonSerialized] public float depthIncrementMultiplier = 1f;
    
    [System.NonSerialized] public float currentOxy = 100f;
    [System.NonSerialized] public float oxyIncrementMultiplier = 1f;
    
    [System.NonSerialized] public float startTime;

    private void Increment()
    {
        this.currentDepth += this.depthBaseIncrementAmount * depthIncrementMultiplier;
        
        this.startTime = Time.time;
    }

    void Start()
    {
        this.startTime = Time.time;
    }

    void Update()
    {
        if (Time.time - this.startTime >= this.thresholdTime)
        {
            this.Increment();
        }

        depthCorrespondingText.text = this.depthLeadingText + this.currentDepth.ToString("F2") + this.depthPostText;
        oxyCorrespondingText.text = this.oxyLeadingText + this.currentOxy.ToString("F0") + this.oxyPostText;
    }
}
