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
    public float baseIncrementAmount = 10;
    
    [Header("UI")]
    public TMP_Text correspondingText;
    public string leadingText = "";
    public string postText = "";
    
    [System.NonSerialized] public float currentLevel = 4f;
    [System.NonSerialized] public float incrementMultiplier = 1f;
    [System.NonSerialized] public float startTime;

    private void IncrementLevel()
    {
        this.currentLevel += this.baseIncrementAmount * incrementMultiplier;
        this.startTime = Time.time;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - this.startTime >= this.thresholdTime)
        {
            this.IncrementLevel();
        }

        correspondingText.text = this.leadingText + this.currentLevel.ToString("F2") + this.postText;
    }
}
