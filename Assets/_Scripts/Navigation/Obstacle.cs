using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("External References")]
    public LineRenderer lineRenderer;
    public List<Transform> edgeLocations;
    
    [Header("Stats")]
    public float speed = 1f;
    public int numHeightLines = 3;
    public float steepness = 1f;

    private void CreateHeightLines()
    {
        for (int i = 0; i < numHeightLines - 1; i++)
        {
            var x = Instantiate(this.lineRenderer.gameObject, this.transform);
            x.transform.localScale *= (i + 1) / (float)numHeightLines;
        }
    }
    
    private void InitializeBorders()
    {
        this.lineRenderer.positionCount = 0;
        Vector3[] points = new Vector3[this.edgeLocations.Count];
        for (int i = 0; i < this.edgeLocations.Count; i++)
        {
            float xPos = this.edgeLocations[i].localPosition.x;
            float zPos = this.edgeLocations[i].localPosition.z;
            points[i] = new Vector3(xPos, 0f, zPos);
        }

        this.lineRenderer.positionCount = edgeLocations.Count;
        this.lineRenderer.loop = true;
        this.lineRenderer.SetPositions(points);
    }
    
    void Start()
    {
        this.InitializeBorders();
        this.CreateHeightLines();
    }

    void Update()
    {
        float dt = Time.deltaTime;
        this.transform.Translate(new Vector3(0f, 0f, -1f) * (dt * this.speed), Space.World);
    }
}
