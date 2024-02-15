using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Scope : MonoBehaviour, IInteractable
{
    public Camera scopeCamera;
    public Player player;
    public Image crosshair;

    public float sensitivity = 0.2f;

    public float minimumX = -360f;
    public float maximumX = 360f;
    public float minimumY = -360f;
    public float maximumY = 360;

    private float _xDelta = 0f;
    private float _yDelta = 0f;

    private float _rotationX = 0f;
    private float _rotationY = 0f;
    
    private Enemy _lastTargeted;

    public void OnLookVertical(InputValue value)
    {
        this._yDelta = value.Get<float>();
    }

    public void OnLookHorizontal(InputValue value)
    {
        this._xDelta = value.Get<float>();
    }

    public void OnUse()
    {
        this.Deactivate();
        player.Activate();
    }

    public void Interact()
    {
        this.Activate();
    }

    private void Activate()
    {
        this.scopeCamera.enabled = true;
        this.GetComponent<PlayerInput>().enabled = true;
        this.crosshair.enabled = true;
    }

    private void Deactivate()
    {
        this.scopeCamera.enabled = false;
        this.GetComponent<PlayerInput>().enabled = false;
        this.crosshair.enabled = false;
    }

    private void CheckTarget(float dt)
    {
        RaycastHit hit;
        Ray ray = scopeCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000f))
        {
            Enemy e = hit.transform.GetComponentInParent<Enemy>();
            if (e != null)
            {
                e.MousedOver(dt);
                this._lastTargeted = e;
            }
        }
        else if (this._lastTargeted != null)
        {
            this._lastTargeted.ResetTargeting();
            this._lastTargeted = null;
        }
    }
    
    void Start()
    {
        
    }

    private void UpdateRotation()
    {
        this._rotationX += this._xDelta * this.sensitivity;
        this._rotationX = Mathf.Clamp(this._rotationX, this.minimumX, this.maximumX);

        this._rotationY += this._yDelta * this.sensitivity;
        this._rotationY = Mathf.Clamp(this._rotationY, this.minimumY, this.maximumY);

        this.scopeCamera.transform.localEulerAngles = new Vector3(-this._rotationY, this._rotationX, 0);
    }
    
    void Update()
    {
        float dt = Time.deltaTime;
        
        this.UpdateRotation();
        this.CheckTarget(dt);
    }
}
