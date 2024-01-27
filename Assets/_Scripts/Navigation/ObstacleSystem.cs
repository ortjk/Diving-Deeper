using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSystem : MonoBehaviour
{
    public ShipControls shipControls;

    public List<Obstacle> obstaclePrefabs;

    private void CreateNewObstacle()
    {
        int i = Random.Range(0, obstaclePrefabs.Count);
        GameObject obj = Instantiate(this.obstaclePrefabs[i].gameObject, this.transform);
        obj.GetComponent<Obstacle>().shipControls = this.shipControls;
        obj.transform.Translate(20f, 0f, 180f);
    }
    
    void Start()
    {
        this.CreateNewObstacle();
    }

    void Update()
    {
        
    }
}
