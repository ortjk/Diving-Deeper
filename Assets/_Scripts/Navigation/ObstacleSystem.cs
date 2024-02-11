using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSystem : MonoBehaviour
{
    [Header("External References")]
    public ShipControls shipControls;
    public LevelTimer levelTimer;
    public List<Obstacle> obstaclePrefabs;

    [Header("Stats")] 
    public float minDepthToSpawn = 50f;
    public float xBound = 225f;

    private float _lastSpawnedDepth = 0f;

    private void CreateNewObstacle()
    {
        int i = Random.Range(0, obstaclePrefabs.Count);
        GameObject obj = Instantiate(this.obstaclePrefabs[i].gameObject, this.transform);
        obj.GetComponent<Obstacle>().shipControls = this.shipControls;

        float x = Random.Range(-this.xBound, this.xBound);
        obj.transform.Translate(x, 0f, 200f);
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        if (this.levelTimer.currentLevel - this._lastSpawnedDepth >= this.minDepthToSpawn)
        {
            this.CreateNewObstacle();
            this._lastSpawnedDepth = this.levelTimer.currentLevel;
        }
    }
}