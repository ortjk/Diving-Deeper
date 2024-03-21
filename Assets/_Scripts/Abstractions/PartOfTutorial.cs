using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PartOfTutorial : MonoBehaviour
{
    public bool finishedTutorial;
    
    protected void Start()
    {
        this.finishedTutorial = TutorialSequence.Complete;
    }
}
