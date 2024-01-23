using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthShader : MonoBehaviour
{
    [Header("External References")]
    public NavigationBackground navigationBackground;
    public LineSystem lineSystem;

    private float[] _quadrantConcentrations = new float[4];
    private Renderer _renderer;
    
    void Start()
    {
        this._renderer = this.GetComponent<Renderer>();
        this._renderer.material.SetVector("_Highest_Point", this.navigationBackground.GetHighestPoint());
        this._renderer.material.SetVector("_Lowest_Point", this.navigationBackground.GetLowestPoint());
    }

    void Update()
    {
        
    }
}
