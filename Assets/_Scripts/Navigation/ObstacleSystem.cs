using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSystem : MonoBehaviour
{
    [Header("External References")]
    public ShipControls shipControls;
    public LevelTimer levelTimer;
    public List<Obstacle> obstaclePrefabs;
    public List<TurbinePLC> plcs = new List<TurbinePLC>();
    public CharacterController characterController;

    [Header("Stats")] 
    public float minDepthToSpawn = 50f;
    public List<float> lanesXPos = new List<float>();

    [Header("Spawn Variance")] 
    public float maxDepthVariance = 25f;
    public float maxPositionVariance = 25f;
    public float maxRotationVariance = 10f;
    public float maxScaleVariance = 0.2f;

    private float _lastSpawnedDepth = 0f;

    private static float NormalDistribution(float x, float mean=1f, float std=0.4f)
    {
        if (std == 0f)
        {
            std = 1f;
        }
        
        float e = 2.71828183f;
        return (1 / (std * Mathf.Pow(2f * Mathf.PI, 0.5f))) * Mathf.Pow(e, -0.5f * Mathf.Pow((x - mean) / std, 2f));
    }

    private static int RandomSign()
    {
        return Random.value < .5 ? 1 : -1;
    }
    
    private void CreateNewObstacle()
    {
        int i = Random.Range(0, obstaclePrefabs.Count);
        GameObject obj = Instantiate(this.obstaclePrefabs[i].gameObject, this.transform);

        Obstacle obst = obj.GetComponent<Obstacle>();
        obst.shipControls = this.shipControls;
        obst.characterController = this.characterController;
        foreach (TurbinePLC t in this.plcs)
        {
            obst.OnHit += t.Unsync;
        }
        
        // set position
        int x = Random.Range(0, obst.elligbleLanes.Count);
        int laneIndex = obst.elligbleLanes[x];
        obj.transform.Translate(this.lanesXPos[laneIndex], 0f, 500f, Space.World);
        
        // add position and rotation variance
        float rm = NormalDistribution(Random.Range(0f, 2f));
        obj.transform.Translate(rm * this.maxPositionVariance * RandomSign(), 0f, 0f, Space.World);
        obj.transform.Rotate(0f, rm * this.maxRotationVariance * RandomSign(), 0f);

        float s = (1f + rm * this.maxScaleVariance * RandomSign()) * obj.transform.localScale.x;
        obj.transform.localScale = new Vector3(s, obj.transform.localScale.y, s);

        this._lastSpawnedDepth += rm * this.maxDepthVariance * RandomSign();
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        if (this.levelTimer.currentDepth - this._lastSpawnedDepth >= this.minDepthToSpawn)
        {
            this._lastSpawnedDepth = this.levelTimer.currentDepth;
            this.CreateNewObstacle();
        }
    }
}
