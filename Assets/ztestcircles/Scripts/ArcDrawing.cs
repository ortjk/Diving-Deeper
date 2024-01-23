using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ArcDrawing : MonoBehaviour
{
    public Vector3 mAngle;
    
    // Start is called before the first frame update
    void Start()
    {
        Handles.DrawSolidArc(this.transform.position, Vector3.forward, Vector3.up, mAngle.y * -360f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
