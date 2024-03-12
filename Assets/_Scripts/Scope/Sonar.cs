using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonar : MonoBehaviour
{
    [Header("External References")]
    public EnemySpawner enemySpawner;
    public GameObject pingIcon;

    [Header("Internal References")] 
    public Transform sweeper;

    [Header("Stats")] 
    public float turnSpeed = 1f;
    public float sonarMaxDistance = 10f;

    private Dictionary<Enemy, GameObject> pings = new Dictionary<Enemy, GameObject>();
    private float _currentRotation = 0f;

    public void RemovePing(Enemy enemy)
    {
        if (this.pings.ContainsKey(enemy))
        {
            Destroy(this.pings[enemy]);
            this.pings.Remove(enemy);
        }
    }
    
    void Start()
    {
        
    }

    private void CastRay()
    {
        float xDir = Mathf.Sin(this._currentRotation * Mathf.Deg2Rad);
        float zDir = Mathf.Cos(this._currentRotation * Mathf.Deg2Rad);

        Vector3 rayOrigin = this.enemySpawner.transform.position;
        int layerMask = 1 << 10;
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, new Vector3(xDir, 0f, zDir), out hit, this.enemySpawner.spawnDistance * 1.2f, layerMask))
        {
            Enemy e = hit.collider.transform.GetComponentInParent<Enemy>();
            if (!pings.ContainsKey(e))
            {
                GameObject p = Instantiate(pingIcon, this.transform);
                pings.Add(e, p);
            }

            float ld = Mathf.InverseLerp(0f, this.enemySpawner.spawnDistance, Vector3.Distance(rayOrigin, hit.collider.transform.position));
            float nd = Mathf.Lerp(0f, this.sonarMaxDistance, ld);
            pings[e].transform.localPosition = new Vector3(xDir * nd, 0f, zDir * nd);
        }
    }

    void Update()
    {
        float dt = Time.deltaTime;

        this._currentRotation += turnSpeed * dt;
        if (this._currentRotation >= 360f)
        {
            this._currentRotation = 0f;
        }
        
        this.CastRay();
        this.sweeper.localRotation = Quaternion.Euler(0f, this._currentRotation, 0f);
    }
}
