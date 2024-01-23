using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTest : MonoBehaviour
{
    public float rotationSpeed;
    
    private BuildCircleMesh _circ;
    
    // Start is called before the first frame update
    void Start()
    {
        _circ = this.GetComponent<BuildCircleMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
        
        this.transform.Rotate(0, 0, rotationSpeed * deltaTime, Space.Self);
    }
}
