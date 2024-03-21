using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

public class Enemy : MonoBehaviour, ICanBeHit
{
    [Header("External References")]
    public Transform target;
    public Sonar sonar;
    
    [Header("Internal References")]
    public Canvas canvas;
    public RectTransform crosshairTransform;
    
    [Header("Stats")]
    public float speed = 2f;
    public float ratio = 0.08f;
    public float targetingSpeed = 0.5f;
    public float distanceToHit = 10f;
    
    public event ICanBeHit.HitEvent OnHit;

    private float _targetingProgress = 0f;

    public bool AttemptShot()
    {
        if (_targetingProgress >= 1f)
        {
            return true;
        }

        return false;
    }
    
    public void MousedOver(float dt)
    {
        _targetingProgress += (targetingSpeed * dt);
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

        if (Mathf.Abs(Vector3.Distance(this.transform.position, this.target.transform.position)) <= this.distanceToHit)
        {
            if (this.OnHit != null)
            {
                this.OnHit.Invoke();
            }
            Destroy(this.gameObject);
        }
        
        this.UpdateCrosshair();
    }
    
    void OnDestroy()
    {
        this.OnHit = null;
        this.sonar.RemovePing(this);
    }
}
