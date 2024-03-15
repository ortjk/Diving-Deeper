using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurbinePLC : MonoBehaviour, IInteractable
{
    public Camera PLCCamera;
    public List<Skillcheck> skillchecks;
    public Player player;

    public bool[] skillcheckMoving = new bool[3];
    
    [Header("Sounds")] 
    public FMODUnity.EventReference successSoundEvent;
    public FMODUnity.EventReference failureSoundEvent;
    
    public event IInteractable.Interacted OnInteract;
    public event IInteractable.Interacted OnUninteract;
        
    private int _currentSkillckeck = 0;

    public void OnUse()
    {
        this.Deactivate();
        player.Activate();
    }
    
    public void OnPress()
    {
        if (this.skillcheckMoving[this._currentSkillckeck])
        {
            if (skillchecks[_currentSkillckeck].circle.Press())
            {
                // success

                this.PlaySuccessSound();

                // snap player to center of target
                this.skillchecks[_currentSkillckeck].circle.SnapPlayerToTarget();
                this.skillchecks[_currentSkillckeck].circle.isRotating = false;
                this.skillcheckMoving[_currentSkillckeck] = false;

                if (this._currentSkillckeck < this.skillchecks.Count - 1)
                {
                    this._currentSkillckeck += 1;

                    while (!this.skillcheckMoving[_currentSkillckeck])
                    {
                        this._currentSkillckeck += 1;

                        if (this._currentSkillckeck >= this.skillchecks.Count - 1)
                        {
                            Debug.Log("Complete");
                            break;
                        }
                    }

                    this.skillchecks[_currentSkillckeck].circle.isRotating = true;
                }
                else
                {
                    Debug.Log("Complete");
                }
            }
            else
            {
                // failure
                this.PlayFailureSound();
            }
        }
    }
    
    public void Interact()
    {
        this.Activate();
        
        if (this.OnInteract != null)
        {
            this.OnInteract.Invoke();
        }
    }

    private void Activate()
    {
        this.PLCCamera.enabled = true;
        this.GetComponent<PlayerInput>().enabled = true;
    }

    private void Deactivate()
    {
        this.PLCCamera.enabled = false;
        this.GetComponent<PlayerInput>().enabled = false;
        
        if (this.OnUninteract != null)
        {
            this.OnUninteract.Invoke();
        }
    }

    private void RestartSkillchecks()
    {
        int counter = 0;
        foreach (var skillcheck in skillchecks)
        {
            skillcheck.baseLine.periods = counter + 1;
            skillcheck.baseLine.UpdatePoints();
            skillcheck.playerLine.periods = counter + 1;

            if (skillcheckMoving[counter])
            {
                skillcheck.circle.playerCircle.transform.Rotate(0f, 0f, Random.value * 180 + 20);
            }

            if (counter == _currentSkillckeck)
            {
                if (skillcheckMoving[counter])
                {
                    skillcheck.circle.isRotating = true;
                }
                else
                {
                    _currentSkillckeck += 1;
                }
            }

            counter += 1;
        }
    }

    private void PlaySuccessSound()
    {
        FMOD.Studio.EventInstance eInstance = FMODUnity.RuntimeManager.CreateInstance(this.successSoundEvent);
        eInstance.setParameterByName("Skillchecks Remaining", (this.skillchecks.Count - (this._currentSkillckeck + 1)));

        // play sound
        eInstance.start();
        eInstance.release();
    }

    private void PlayFailureSound()
    {
        FMOD.Studio.EventInstance eInstance = FMODUnity.RuntimeManager.CreateInstance(this.failureSoundEvent);
        
        // play sound
        eInstance.start();
        eInstance.release();
    }
    
    void Start()
    {
        this.RestartSkillchecks();
    }

    void Update()
    {
        
    }
}
