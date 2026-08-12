using System.Collections.Generic;
using UnityEngine;

public class SpawnMinion : MonoBehaviour
{
    [SerializeField] private List<GameObject> notSpawned = new List<GameObject>();
    private List<GameObject> alreadySpawned = new List<GameObject>();
    [SerializeField] private int spawnAmount = 0;
    [SerializeField] GameObject explosiveMosquito;
    [SerializeField] private Transform spawnPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SpawnMinionAction(GameObject gameObject)
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            if (notSpawned.Count > 0)
            {
                int randomEnemy = Random.Range(0, notSpawned.Count);
                Instantiate(notSpawned[randomEnemy], spawnPos);
                alreadySpawned.Add(notSpawned[randomEnemy]);
                notSpawned.RemoveAt(randomEnemy);
            }
            else
            {
                Instantiate(explosiveMosquito, spawnPos);
            }
        }
    }
}
