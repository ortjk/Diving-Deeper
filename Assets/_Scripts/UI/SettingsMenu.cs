using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : Saver
{
    private class SettingSave
    {
        public float volume;
        public float sensitivity;
        public int resolutionIndex;
    }

    [Header("UI Elements")]
    public Slider volumeSlider;
    public Slider sensitivitySlider;
    public TMP_Dropdown dropdown;

    [System.NonSerialized] public static float Sensitivity;

    private FMOD.Studio.Bus _bus;
    private Resolution[] _resolutions;

    private SettingSave _saveData = new SettingSave();

    protected override void SetInitialData()
    {
        this._saveData.volume = 1f;
        this._saveData.sensitivity = 1f;
        this._saveData.resolutionIndex = Screen.resolutions.Length - 1;
        this.SaveData(this._saveData);
    }

    public void SetVolume()
    {
        float vol = this.volumeSlider.value;
        this._bus.setVolume(vol);
        this._saveData.volume = vol;
    }
    
    public void SetSensitivity()
    {
        float sens = this.sensitivitySlider.value;
        Sensitivity = sens;
        this._saveData.sensitivity = sens;
    }

    public void SetResolution()
    {
        int res = this.dropdown.value;
        Screen.SetResolution(_resolutions[res].width, _resolutions[res].height, true);
        this._saveData.resolutionIndex = res;
    }

    public float GetVolume()
    {
        using StreamReader reader = new StreamReader(this.persistentPath);
        string json = reader.ReadToEnd();

        SettingSave ss = JsonUtility.FromJson<SettingSave>(json);

        return ss.volume;
    }

    public float GetSensitivity()
    {
        using StreamReader reader = new StreamReader(this.persistentPath);
        string json = reader.ReadToEnd();

        SettingSave ss = JsonUtility.FromJson<SettingSave>(json);

        return ss.sensitivity;
    }

    public int GetResolutionIndex()
    {
        using StreamReader reader = new StreamReader(this.persistentPath);
        string json = reader.ReadToEnd();

        SettingSave ss = JsonUtility.FromJson<SettingSave>(json);

        return ss.resolutionIndex;
    }
    public void Back()
    {
        this.gameObject.SetActive(false);
    }

    new void Awake()
    {
        this._resolutions = Screen.resolutions;
        this.jsonName = "SaveData.json";
        base.Awake();
    }

    void Start()
    {
        // load saved settings from json
        float savedVol = this.GetVolume();
        float savedSens = this.GetSensitivity();
        int savedRes = this.GetResolutionIndex();

        // volume
        this._bus = FMODUnity.RuntimeManager.GetBus("bus:/");
        this.volumeSlider.value = savedVol;
        this.SetVolume();
        
        // sensitivity
        this.sensitivitySlider.value = savedSens;
        this.SetSensitivity();

        // resolution
        foreach (Resolution r in _resolutions)
        {
            this.dropdown.options.Add(new TMP_Dropdown.OptionData(r.ToString()));
        }
        this.dropdown.value = savedRes;
        this.SetResolution();
        
        this.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        this.SaveData(this._saveData);
    }
}
