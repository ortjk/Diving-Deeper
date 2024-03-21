using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, ICanBeHit
{
    [Header("External References")]
    public LineRenderer lineRenderer;
    public ShipControls shipControls;
    public List<Transform> edgeLocations;
    public CharacterController characterController;

    [Header("Stats")]
    public int numHeightLines = 3;
    public float steepness = 1f;
    public float minZPosition = -250f;
    public List<int> elligbleLanes = new List<int>();

    public event ICanBeHit.HitEvent OnHit;

    private Collider[] _colliders;
    
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ship"))
        {
            if (this.OnHit != null)
            {
                this.OnHit.Invoke();
            }
            Destroy(this.gameObject);
        }
    }
    
    void Start()
    {
        this._colliders = this.GetComponents<Collider>();
        foreach (Collider c in this._colliders)
        {
            this.characterController.IgnoredColliders.Add(c);
        }
        
        this.InitializeBorders();
        this.CreateHeightLines();
    }

    void Update()
    {
        float dt = Time.deltaTime;
        this.transform.Translate(new Vector3(0f, 0f, -1f) * (dt * shipControls.speed), Space.World);

        if (this.transform.position.z <= this.minZPosition)
        {
            Destroy(this.gameObject);
        }
    }

    void OnDestroy()
    {
        this.OnHit = null;
        
        foreach (Collider c in this._colliders)
        {
            this.characterController.IgnoredColliders.Remove(c);
        }
    }
}
