using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

public class Enemy : MonoBehaviour
{
    [Header("Internal References")]
    public Transform target;
    public Canvas canvas;
    public RectTransform crosshairTransform;
    
    [Header("Stats")]
    public float speed = 10f;
    public float ratio = 0.1f;
    public float targetingSpeed = 1f;

    private float _targetingProgress = 0f;

    public void MousedOver(float dt)
    {
        _targetingProgress += (targetingSpeed * dt);

        if (_targetingProgress >= 1f)
        {
            Debug.Log("Destroyed!");
        }
    }

    public void ResetTargeting()
    {
        this._targetingProgress = 0f;
    }

    private Vector3 DirectionToTarget()
    {
        return Vector3.Normalize(target.transform.position - this.transform.position);
    }

    private void UpdateCrosshair()
    {
        this.canvas.transform.LookAt(target.position);

        this.canvas.transform.position = Vector3.Lerp(this.transform.position, target.transform.position, 0.1f);

        float distance = (this.transform.position - target.transform.position).magnitude;

        float scale = distance * ratio;
        this.crosshairTransform.sizeDelta = new Vector2(scale, scale);

        float currentAngle = Mathf.Lerp(0f, 45f, this._targetingProgress);
        Quaternion newRotation = canvas.transform.rotation * Quaternion.Euler(new Vector3(0f, 0f, currentAngle));
        this.crosshairTransform.rotation = newRotation;
    }

    void Start()
    {
        
    }

    void Update()
    {
        float dt = Time.deltaTime;
        
        this.transform.Translate(DirectionToTarget() * (dt * speed), Space.World);
        this.UpdateCrosshair();
    }
}
