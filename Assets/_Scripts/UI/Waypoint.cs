using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [Header("External References")] 
    public Transform goal;

    [Header("Stats")] 
    public Vector3 offset;

    private Camera _cam;
    private RectTransform _rectTransform;
    
    void Start()
    {
        this._cam = Camera.main;
        this._rectTransform = this.gameObject.GetComponent<RectTransform>();
    }

    private void UpdatePosition()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(this._cam);
        Bounds bounds = new Bounds(this.goal.position + this.offset, Vector3.one * 0.01f);
        bool inCamera = GeometryUtility.TestPlanesAABB(planes, bounds);
        
        if (inCamera)
        {
            this._rectTransform.position = this._cam.WorldToScreenPoint(this.goal.position + this.offset);
        }
        else
        {
            Vector3 goalPosition = this.goal.position + this.offset;

            // set image position into camera range
            Vector3 projection = planes[5].ClosestPointOnPlane(goalPosition);
            this._rectTransform.position = this._cam.WorldToScreenPoint(projection);

            // resize image to be visible by bringing it back to a 0 z-position
            // normalize the resulting vector
            Vector3 normalizedLocal = new Vector3(this._rectTransform.localPosition.x, this._rectTransform.localPosition.y, 0f).normalized;
            normalizedLocal = new Vector3(normalizedLocal.x * 900f, normalizedLocal.y * 500f); // resize the normalized vector to screen bounds
            this._rectTransform.localPosition = normalizedLocal;
        }

    }

    void Update()
    {
        this.UpdatePosition();
    }
}
