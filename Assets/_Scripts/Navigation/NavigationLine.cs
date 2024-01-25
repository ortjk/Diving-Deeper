using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationLine : MonoBehaviour
{
    public LineRenderer lineRenderer;
    
    public int segments = 20;
    public float width = 350f;

    [System.NonSerialized] public List<float> zPositions;

    private float _xSpaceBetweenPoints;
    
    public void UpdatePoints()
    {
        Vector3[] points = new Vector3[segments];
        float leftmost = this.transform.localPosition.x - (width / 2);
        for (int i = 0; i < segments; i++)
        {
            float xPos = leftmost + (i * _xSpaceBetweenPoints);
            float zPos = zPositions[i];
            points[i] = new Vector3(xPos, 0f, zPos);
        }

        this.lineRenderer.positionCount = segments;
        this.lineRenderer.SetPositions(points);
    }

    void Awake()
    {
        this.zPositions = new List<float>();
        for (int i = 0; i < segments; i++) { this.zPositions.Add(0f); }
    }
    
    void Start()
    {
        this._xSpaceBetweenPoints = width / (segments - 1);

        this.UpdatePoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
