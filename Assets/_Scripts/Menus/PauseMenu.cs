using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameMenuHandler gameMenuHandler;
    public SettingsMenu settingsMenu;
    
    public void Resume()
    {
        gameMenuHandler.OnPause();
    }

    public void Settings()
    {
        this.settingsMenu.gameObject.SetActive(true);
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnEnable()
    {
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
}
