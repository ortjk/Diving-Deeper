using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuHandler : MonoBehaviour
{
    public PauseMenu pauseMenu;
    public SettingsMenu settingsMenu;

    private bool _paused = false;

    public void OnPause()
    {
        _paused = !_paused;
        this.pauseMenu.gameObject.SetActive(_paused);
        this.settingsMenu.gameObject.SetActive(false);

        if (_paused)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void Update()
    {
        if (!_paused)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
