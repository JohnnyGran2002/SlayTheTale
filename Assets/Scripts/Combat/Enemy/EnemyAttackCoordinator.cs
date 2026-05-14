using NUnit.Framework.Constraints;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackCoordinator : MonoBehaviour
{
    public List<GameObject> AttackQueue = new List<GameObject>();

    public void StartEnemyTurn()
    {
        AttackQueue.Clear();

        List<GameObject> enemies = new List<GameObject>();
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
    }

    private void StartNextEnemy()
    {
        if (AttackQueue.Count == 0)
        {
            EndEnemyTurn();
            return;
        }

        GameObject enemy = AttackQueue[0];
        AttackQueue.Remove(enemy);

        Debug.Log("Enemy " + enemy.name + " is attacking!");

        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.SetState(EnemyAI.EnemyState.Attack);
        }
    }

    private void OnEnemyFinishedAttack()
    {
        StartNextEnemy();
    }

    private void EndEnemyTurn()
    {
        Debug.Log("End enemy turn");
    }
}
