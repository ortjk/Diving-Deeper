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
        
    private int _currentSkillckeck = 0;

    public void OnUse()
    {
        this.Deactivate();
        player.Activate();
    }
    
    public void OnPress()
    {
        if (skillchecks[_currentSkillckeck].circle.Press())
        {
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
    }
    
    public void Interact()
    {
        this.Activate();
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
    
    void Start()
    {
        this.RestartSkillchecks();
    }

    void Update()
    {
        
    }
}
