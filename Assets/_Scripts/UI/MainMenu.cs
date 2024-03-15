using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public SettingsMenu settingsMenu;
    
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Settings()
    {
        this.settingsMenu.gameObject.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
