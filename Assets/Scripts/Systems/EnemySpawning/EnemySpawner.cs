using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject enemiesParent;
    [SerializeField, Tooltip("Click this when testing in scenes. Otherwise you need to enter from the map.")] private bool manualEnemyData;
    public EnemyData enemyData;
    
    private GameObject _enemyToSpawn;
    private void Awake()
    {
        if (!manualEnemyData)
        {
            enemyData = MapGenerator.i.enemyData;
        }
        
        for (var i = 0; i < spawnPoints.Length; i++)
        {
            if (enemyData.enemies[i] == null) continue;
            
            _enemyToSpawn = enemyData.enemies[i];

            Instantiate(_enemyToSpawn, spawnPoints[i].position, Quaternion.identity, enemiesParent.transform);
        }
    }
}
