using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularSkillcheck : MonoBehaviour
{
    [Header("Circles")]
    public BuildCircleMesh targetCircle;
    public BuildCircleMesh playerCircle;

    [Header("Stats")]
    public float playerSpeed;
    public float playerSize;
    public float targetSize;
    public float minTargetDistance;
    
    [System.NonSerialized] public bool isRotating = false;
    [System.NonSerialized] public float playerAngle;
    [System.NonSerialized] public float targetAngle;

    private float _playerDirection = 1f;

    public static bool CheckCirclesOverlap(BuildCircleMesh circle1, BuildCircleMesh circle2)
    {
        float angle1 = circle1.transform.localEulerAngles.z;
        float angle2 = circle2.transform.localEulerAngles.z;
        float dAngle = Mathf.Abs(Mathf.DeltaAngle(angle1, angle2));

        if (dAngle <= (circle1.endAngle / 2 + circle2.endAngle / 2))
        {
            return true;
        }
        return false;
    }

    public static float GetAngleBetween360(float angle)
    {
        return ((angle % 360f) + 360f) % 360f;
    }

    public void SnapPlayerToTarget()
    {
        Quaternion dAngle = Quaternion.Euler(0, 0, -(this.targetCircle.endAngle - this.targetCircle.startAngle)/2);
        this.playerCircle.transform.rotation = this.targetCircle.transform.rotation * dAngle;
    }
    
    public bool Press()
    {
        if (CheckCirclesOverlap(this.playerCircle, this.targetCircle))
        {
            return true;
        }
        
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerCircle.startAngle = 0f;
        playerCircle.endAngle = playerSize;
        playerCircle.transform.parent.Rotate(0, 0, playerSize / 2);
        
        targetCircle.startAngle = 0f;
        targetCircle.endAngle = targetSize;
        targetCircle.transform.parent.Rotate(0, 0, targetSize / 2);

        this.targetAngle = targetCircle.startAngle;
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;

        if (this.isRotating)
        {
            playerCircle.transform.Rotate(0, 0, _playerDirection * playerSpeed * deltaTime, Space.Self);
        }
        
        this.playerAngle = playerCircle.transform.localEulerAngles.z;
    }
}
