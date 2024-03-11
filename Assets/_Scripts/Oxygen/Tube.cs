using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tube : MonoBehaviour
{
    [Header("Internal References")]
    public Renderer renderer;
    public Transform canvas;
    public TMP_Text percentage;

    private Transform _playerCameraPosition;

    public void SetFill(float fill)
    {
        fill = Mathf.Clamp01(fill);
        
        this.renderer.material.SetFloat("_fill", fill);
        this.percentage.text = (int)(fill * 100f) + "%";
    }

    public void Start()
    {
        this._playerCameraPosition = Camera.main.transform;
    }

    public void Update()
    {
        this.canvas.LookAt(this._playerCameraPosition);
    }
}
