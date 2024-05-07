using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NavigationBackground : MonoBehaviour
{
    [Header("External References")] 
    public ShipControls shipControls;
    
    [Header("Stats")]
    public float maxAngle = 25f;
    public float minAngle = 3f;

    [System.NonSerialized] public float fractionRotation;
    
    [Header("Developer Options")]
    [SerializeField] private bool _debug = false;
    
    private float _length;
    private float _targetRotation;

    public Vector3 GetHighestPoint()
    {
        float rotation = this.maxAngle * Mathf.Deg2Rad;
        float zOffset = (_length / 2) * Mathf.Cos(rotation);
        float yOffset = (_length / 2) * Mathf.Sin(rotation);

        return this.transform.position + new Vector3(0f, yOffset, -zOffset);
    }
    
    public Vector3 GetLowestPoint()
    {
        float rotation = this.maxAngle * Mathf.Deg2Rad;
        float zOffset = (_length / 2) * Mathf.Cos(rotation);
        float yOffset = (_length / 2) * Mathf.Sin(rotation);

        return this.transform.position + new Vector3(0f, -yOffset, zOffset);
    }

    void Awake()
    {
        this._length = this.transform.localScale.z * 10;

        if (!_debug)
        {
            this.transform.rotation = Quaternion.Euler(Random.Range(this.minAngle, this.maxAngle), 0f, 0f);
            this._targetRotation = this.maxAngle;
        }
        this.UpdateFractionRotation();
    }
    
    void Start()
    {
        
    }

    private void UpdateFractionRotation()
    {
        this.fractionRotation = (this.transform.rotation.eulerAngles.x - this.minAngle ) / this.maxAngle;
    }
    
    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        float xRotation = this.transform.localRotation.eulerAngles.x;

        this.UpdateFractionRotation();

        if (xRotation > maxAngle)
        {
            this.transform.rotation = Quaternion.Euler(this.maxAngle, 0f, 0f);
        }
    
        if (xRotation < minAngle)
        {
            this.transform.rotation = Quaternion.Euler(this.minAngle, 0f, 0f);
        }
        
        if (!_debug)
        {
            float rotDiff = this._targetRotation - xRotation;
            if (Mathf.Abs(rotDiff) <= 1f)
            {
                this._targetRotation = Random.Range(minAngle, maxAngle);
            }
            else
            {
                if (rotDiff > 0)
                {
                    this.transform.Rotate(new Vector3(shipControls.speed / 10 * dt, 0f, 0f), Space.Self);
                }
                else
                {
                    this.transform.Rotate(new Vector3(- shipControls.speed / 10 * dt, 0f, 0f), Space.Self);
                }
            }
        }
    }
}
