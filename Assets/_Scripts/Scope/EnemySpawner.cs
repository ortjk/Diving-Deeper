using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("External References")] 
    public Scope scope;
    public LevelTimer timer;
    public Sonar sonar;
    
    public List<Enemy> enemyPrefabs = new List<Enemy>();

    [Header("Stats")]
    public float spawnDistance = 100f;
    public float avgEnemiesPerMinute = 2f;

    private float _timeOfLastEnemy = 0f;
    private float _timeForNextEnemy = 0f;

    public void SpawnEnemy()
    {
        float theta = Random.Range(this.scope.minimumX + 10f, this.scope.maximumX - 10f) * Mathf.Deg2Rad;
        float xPos = this.spawnDistance * Mathf.Sin(theta);
        float zPos = this.spawnDistance * Mathf.Cos(theta);

        int i = Random.Range(0, enemyPrefabs.Count);
        Enemy e = Instantiate(this.enemyPrefabs[i].gameObject, this.transform).GetComponent<Enemy>();
        e.transform.Translate(xPos, 0f, zPos);
        e.target = this.transform;
        e.sonar = this.sonar;
        e.transform.LookAt(this.transform);
        e.speed *= 1f + Mathf.InverseLerp(0f, 4000f, this.timer.currentDepth);

        this._timeOfLastEnemy = Time.time;
        this._timeForNextEnemy = (60f / avgEnemiesPerMinute) * Random.Range(0.75f, 1.25f) / (1f + Mathf.InverseLerp(0f, 4000f, this.timer.currentDepth));
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        if (Time.time - this._timeOfLastEnemy >= this._timeForNextEnemy)
        {
            this.SpawnEnemy();
        }
    }
}
