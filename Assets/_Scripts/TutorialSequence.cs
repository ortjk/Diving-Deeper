using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TutorialSequence : MonoBehaviour
{
    private class TutorialSave
    {
        public bool tutorialComplete;
    }

    [Header("External References")] 
    public Transform t1;

    [Header("Internal References")] 
    public Canvas canvas;

    private TutorialSave _tutorialSave = new TutorialSave();
    private string _path;
    private string _persistentPath;

    public bool IsComplete()
    {
        Debug.Log(this._persistentPath);
        using StreamReader reader = new StreamReader(this._persistentPath);
        string json = reader.ReadToEnd();

        TutorialSave ts = JsonUtility.FromJson<TutorialSave>(json);

        return ts.tutorialComplete;
    }

    public void SetCompletion(bool c)
    {
        this._tutorialSave.tutorialComplete = c;
        this.SaveData();
    }
    
    private void SaveData()
    {
        string json = JsonUtility.ToJson(this._tutorialSave);

        using StreamWriter writer = new StreamWriter(this._persistentPath);
        writer.Write(json);
    }

    private void InitSave()
    {
        this._path = Application.dataPath + Path.AltDirectorySeparatorChar + "Tutorial.json";
        this._persistentPath = Application.persistentDataPath + Path.DirectorySeparatorChar + "Tutorial.json";

        if (!File.Exists(this._persistentPath))
        {
            this._tutorialSave.tutorialComplete = false;
            this.SaveData();
        }
    }

    void Awake()
    {
        this.InitSave();
    }

    void Start()
    {
        if (this.IsComplete())
        {
            this.canvas.gameObject.SetActive(false);
        }
    }
}
