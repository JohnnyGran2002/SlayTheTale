using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpawnMinion : MonoBehaviour
{
    [SerializeField] private List<GameObject> notSpawned = new List<GameObject>();
    private List<GameObject> alreadySpawned = new List<GameObject>();
    [SerializeField] private int spawnAmount = 0;
    [SerializeField] GameObject explosiveMosquito;
    [SerializeField] private Transform spawnPosistion;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SpawnMinionAction()
    {
        Transform spawnPos = spawnPosistion;
        for (int i = 0; i < spawnAmount; i++)
        {
            if (notSpawned.Count > 0)
            {
                int randomEnemy = Random.Range(0, notSpawned.Count);
                Instantiate(notSpawned[randomEnemy], spawnPos.position, Quaternion.identity);
                alreadySpawned.Add(notSpawned[randomEnemy]);
                notSpawned.RemoveAt(randomEnemy);
            }
            else
            {
                Instantiate(explosiveMosquito, spawnPos.position, Quaternion.identity);
            }
        }
    }
}
