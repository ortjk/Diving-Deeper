using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSinLine : SinLine
{
    public CircularSkillcheck skillcheck;

    private float BellCurvedAngle(float angle, float maxX=180)
    {
        float e = 2.71828183f;
        return Mathf.Pow(e, -Mathf.Pow(angle - 180, 2) / 9000);
    }
    
    protected override void Start()
    {
        this.lineRenderer = this.GetComponent<LineRenderer>();
    }

    protected override void Update()
    {
        float angleDiff = Mathf.Abs(this.skillcheck.playerAngle - this.skillcheck.targetAngle);
        this.xOffset = - angleDiff * Mathf.Deg2Rad;
        this.yMultiplier = 1 - BellCurvedAngle(angleDiff);
        
        this.UpdatePoints();
    }
}
