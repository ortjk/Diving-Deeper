using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    public float thresholdTime = 2f;
    public uint incrementAmount = 10;
    
    public TMP_Text correspondingText;
    public string leadingText = "";
    public string postText = "";
    
    [System.NonSerialized] public uint currentLevel = 0;
    [System.NonSerialized] public float startTime;

    private void IncrementLevel()
    {
        this.currentLevel += this.incrementAmount;
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

        correspondingText.text = this.leadingText + this.currentLevel + this.postText;
    }
}
