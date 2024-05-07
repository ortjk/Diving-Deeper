using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Saver : MonoBehaviour
{
    protected string jsonName; // e.g. "NAME.json"
    protected string path;
    protected string persistentPath;

    protected void SaveData(object saveData)
    {
        string json = JsonUtility.ToJson(saveData);

        using StreamWriter writer = new StreamWriter(this.persistentPath);
        writer.Write(json);
    }

    protected virtual void SetInitialData() { }

    protected void InitSave()
    {
        this.path = Application.dataPath + Path.AltDirectorySeparatorChar + this.jsonName;
        this.persistentPath = Application.persistentDataPath + Path.DirectorySeparatorChar + this.jsonName;

        if (!File.Exists(this.persistentPath))
        {
            this.SetInitialData();
        }
    }

    protected void Awake()
    {
        this.InitSave();
    }
    
    /*
     Example initial data setter class:
     protected override void SetInitialData()
     {
        this._saveData.volume = 1f;
        this.SaveData(this._saveData);
     }
     
     Example setter class:
     public void SetVolume()
     {
        float vol = this.volumeSlider.value;
        this._bus.setVolume(vol);
        this._saveData.volume = vol;
     }
     
     Example getter class:
     public float GetVolume()
     {
        using StreamReader reader = new StreamReader(this.persistentPath);
        string json = reader.ReadToEnd();

        SettingSave ss = JsonUtility.FromJson<SettingSave>(json);

        return ss.volume;
     }
     */
}
