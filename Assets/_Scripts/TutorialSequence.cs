using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Target
{
    public Transform transform;
    [TextAreaAttribute(10,12)]
    public string message;
    public float yOffset;
}

public class TutorialSequence : Saver
{
    private class TutorialSave
    {
        public bool tutorialComplete;
    }

    [Header("Internal References")] 
    public Canvas canvas;

    [Header("Prefabs")] 
    public Waypoint waypointPrefab;
    public TMP_Text messagePrefab;

    [Header("Sequence")]
    public Target[] targets;
    public int pInS = 0; // "position in sequence"

    [NonSerialized] public static bool Complete;
    
    private Waypoint _currentWaypoint;
    private TMP_Text _currentMessage;
    private IInteractable _currentTarget;

    private TutorialSave _tutorialSave = new TutorialSave();

    protected override void SetInitialData()
    {
        this._tutorialSave.tutorialComplete = false;
        this.SaveData(this._tutorialSave);
    }

    public bool IsComplete()
    {
        using StreamReader reader = new StreamReader(this.persistentPath);
        string json = reader.ReadToEnd();

        TutorialSave ts = JsonUtility.FromJson<TutorialSave>(json);

        return ts.tutorialComplete;
    }

    private void SetCompletion(bool c)
    {
        this._tutorialSave.tutorialComplete = c;
        this.SaveData(this._tutorialSave);
    }

    private void OnTargetInteracted()
    {
        Destroy(this._currentWaypoint.gameObject);
        GameObject g = Instantiate(this.messagePrefab.gameObject, this.canvas.transform);
        
        TMP_Text text = g.GetComponent<TMP_Text>();
        this._currentMessage = text;
        text.text = this.targets[pInS].message;
        
        this.pInS += 1;
        _currentTarget.OnInteract -= this.OnTargetInteracted;
    }

    private void OnTargetUninteracted()
    {
        _currentTarget.OnUninteract -= this.OnTargetUninteracted;
        
        this._currentTarget = null;
        Destroy(this._currentMessage.gameObject);

        if (this.pInS >= this.targets.Length)
        {
            this.SetCompletion(true);
            Complete = true;
            Destroy(this.gameObject);
        }
        else
        {
            this.SetWaypoint(this.targets[this.pInS]);
        }
    }

    private void SetWaypoint(Target t)
    {
        GameObject g = Instantiate(this.waypointPrefab.gameObject, this.canvas.transform);
        
        Waypoint w = g.GetComponent<Waypoint>();
        this._currentWaypoint = w;
        w.goal = t.transform;
        w.offset = new Vector3(0f, t.yOffset, 0f);

        this._currentTarget = t.transform.gameObject.GetComponent<IInteractable>();
        this._currentTarget.OnInteract += this.OnTargetInteracted;
        this._currentTarget.OnUninteract += this.OnTargetUninteracted;
    }

    new void Awake()
    {
        this.jsonName = "Tutorial.json";
        base.Awake();
        Complete = this.IsComplete();
    }
    
    void Start()
    {
        if (this.IsComplete())
        {
            Destroy(this.gameObject);
        }
        else
        {
            this.SetWaypoint(targets[pInS]);
        }
    }
}
