using NUnit.Framework.Constraints;
using System.Collections.Generic;
using UnityEngine;
using static Projectile;

public class EnemyAttackCoordinator : MonoBehaviour
{
    public List<GameObject> AttackQueue = new List<GameObject>();

    private List<GameObject> enemies = new List<GameObject>();

    public void StartEnemyTurn()
    {
        // Clear the attack queue and populate it with alive enemies
        AttackQueue.Clear();
        enemies.Clear();

        // Find all enemies in the scene and add them to the list if they are alive
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy.GetComponent<Health>().IsAlive)
            {
                enemies.Add(enemy);
            }
        }

        // Add the alive enemies to the attackQueue
        foreach (GameObject enemy in enemies)
        {
            AttackQueue.Add(enemy);
        }

        //Shuffle the attackQueue to randomize the order of enemy attacks
        AttackQueue.Shuffle();

        StartNextEnemy();
    }

    private void StartNextEnemy()
    {
        // If there are no more enemies to attackQueue, end the enemy turn
        if (AttackQueue.Count == 0)
        {
            EndEnemyTurn();
            return;
        }

        // Get the next enemy from the attackQueue and set it to attack
        GameObject enemy = AttackQueue[0];
        // Remove the enemy from the attackQueue
        AttackQueue.RemoveAt(0);

        Debug.Log("Enemy " + enemy.name + " is attacking!");

        // Set the enemy to attack
        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.SetState(EnemyAI.EnemyState.Attack);
        }
    }

    public void EnemyFinishedAttack()
    {
        StartNextEnemy();
    }

    private void EndEnemyTurn()
    {
        Debug.Log("End enemy turn");
    }
}
