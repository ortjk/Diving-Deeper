using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NavigationBackground : MonoBehaviour
{
    public float maxAngle = 25f;
    public float minAngle = 3f;
    public float rotationSpeed = 1f;
    
    private Renderer _renderer;
    private float _length;
    private float _targetRotation;

    public float GetFractionRotation()
    {
        return (this.transform.rotation.eulerAngles.x - this.minAngle ) / this.maxAngle;
    }

    private Vector3 GetHighestPoint()
    {
        float rotation = this.maxAngle * Mathf.Deg2Rad;
        float zOffset = (_length / 2) * Mathf.Cos(rotation);
        float yOffset = (_length / 2) * Mathf.Sin(rotation);

        return this.transform.position + new Vector3(0f, yOffset, -zOffset);
    }
    
    private Vector3 GetLowestPoint()
    {
        float rotation = this.maxAngle * Mathf.Deg2Rad;
        float zOffset = (_length / 2) * Mathf.Cos(rotation);
        float yOffset = (_length / 2) * Mathf.Sin(rotation);

        return this.transform.position + new Vector3(0f, -yOffset, zOffset);
    }
    
    void Start()
    {
        this.transform.rotation = Quaternion.Euler(this.maxAngle, 0f, 0f); // Quaternion.Euler(Random.Range(this.minAngle, this.maxAngle), 0f, 0f);
        this._targetRotation = this.maxAngle;
        
        this._renderer = this.GetComponent<Renderer>();
        this._length = this.transform.localScale.x * 10;
        
        this._renderer.material.SetVector("_Highest_Point", this.GetHighestPoint());
        this._renderer.material.SetVector("_Lowest_Point", this.GetLowestPoint());
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        float xRotation = this.transform.rotation.eulerAngles.x;
        
        if (xRotation > maxAngle)
        {
            this.transform.rotation = Quaternion.Euler(this.maxAngle, 0f, 0f);
        }
        
        if (xRotation < minAngle)
        {
            this.transform.rotation = Quaternion.Euler(this.minAngle, 0f, 0f);
        }

        float rotDiff = this._targetRotation - xRotation;
        if (Mathf.Abs(rotDiff) <= 1f)
        {
            this._targetRotation = Random.Range(minAngle, maxAngle);
        }
        else
        {
            if (rotDiff > 0)
            {
                this.transform.Rotate(new Vector3(this.rotationSpeed * dt, 0f, 0f), Space.Self);
            }
            else
            {
                this.transform.Rotate(new Vector3(- this.rotationSpeed * dt, 0f, 0f), Space.Self);
            }
        }
        
        Debug.Log(this._targetRotation);
    }
}
