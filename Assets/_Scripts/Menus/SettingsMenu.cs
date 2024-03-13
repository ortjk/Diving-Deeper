using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public void OnUnpause()
    {
        this.Back();
    }
    
    public void Back()
    {
        this.gameObject.SetActive(false);
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
