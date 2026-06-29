using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;

    public EnemyData enemyData;
    
    private GameObject _enemyToSpawn;

    [SerializeField] private GameObject enemiesParent;

    
    private void Awake()
    {
        for (var i = 0; i < spawnPoints.Length; i++)
        {
            if (enemyData.enemies[i] == null) continue;
            
            _enemyToSpawn = enemyData.enemies[i];

            Instantiate(_enemyToSpawn, spawnPoints[i].position, Quaternion.identity, enemiesParent.transform);
        }
    }
}
