using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    [SerializeField] private List<GameObject> gameObjects = new List<GameObject>();
    [SerializeField] private int spawnCount = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Spawn(GameObject gameObject)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            int randomEnemy = Random.Range(0, gameObjects.Count);
            Instantiate(gameObjects[randomEnemy]);
            gameObjects.RemoveAt(randomEnemy);
        }
    }
}
