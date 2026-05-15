using NUnit.Framework.Constraints;
using System.Collections.Generic;
using UnityEngine;
using static Projectile;

public class EnemyAttackCoordinator : MonoBehaviour
{
    public List<GameObject> AttackQueue = new List<GameObject>();

    private EnemyAI enemyAI;

    private List<GameObject> enemies = new List<GameObject>();

    private void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
    }

    public void StartEnemyTurn()
    {
        AttackQueue.Clear();
        enemies.Clear();

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy.GetComponent<Health>().IsAlive)
            {
                enemies.Add(enemy);
            }
        }

        foreach (GameObject enemy in enemies)
        {
            AttackQueue.Add(enemy);
        }

        AttackQueue.Shuffle();

        StartNextEnemy();
    }

    private void StartNextEnemy()
    {
        if (AttackQueue.Count == 0)
        {
            EndEnemyTurn();
            return;
        }

        GameObject enemy = AttackQueue[0];
        AttackQueue.RemoveAt(0);

        Debug.Log("Enemy " + enemy.name + " is attacking!");

        if (enemyAI != null)
        {
            enemyAI.SetState(EnemyAI.EnemyState.Attack);
        }
    }

    public void EnemyFinishedAttack()
    {
        enemyAI.SetState(EnemyAI.EnemyState.Wander);
        StartNextEnemy();
    }

    private void EndEnemyTurn()
    {
        Debug.Log("End enemy turn");
    }
}
