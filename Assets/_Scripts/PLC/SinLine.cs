using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SinLine : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public float width = 5f;
    public float height = 5f;
    public int segments = 10;
    public int periods = 1;

    public float xOffset = 0f;
    public float yMultiplier = 1f;
    
    public void UpdatePoints()
    {
        Vector3[] points = new Vector3[segments];
        for (int i = 0; i < segments; i++)
        {
            float x = ((float)i) / (segments - 1);
            float xPos = x * width;
            float yPos = Mathf.Sin(periods * 2 * Mathf.PI * x + xOffset) * yMultiplier * (height / 2);
            points[i] = new Vector3(xPos, yPos);
        }

        this.lineRenderer.positionCount = segments;
        this.lineRenderer.SetPositions(points);
    }
    
    protected virtual void Start()
    {
        this.lineRenderer = this.GetComponent<LineRenderer>();

        this.UpdatePoints();
    }

    protected virtual void Update()
    {
        
    }
}
