using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Projectile;

public class EnemyAttackCoordinator : MonoBehaviour
{
    public List<GameObject> AttackQueue = new List<GameObject>();

    private List<GameObject> enemies = new List<GameObject>();

    private EnemyAI enemyAI;

    [SerializeField] private GameEvent _ping;

    public void StartEnemyTurn(Component sender, object data)
    {
        if (data is not TurnManager.CurrentTurn.EnemyTurn)
        {
            return;
        }
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
            enemyAI = enemy.GetComponent<EnemyAI>();
            enemyAI.SetState(EnemyStates.Wander);
        }

        //Shuffle the attackQueue to randomize the order of enemy attacks
        AttackQueue.Shuffle();
        _ping.Raise(this, null); // set turn managet to enemy turn active
    }

    public void StartNextEnemyEvent(Component sender, object data)
    {
        if (data is not TurnManager.CurrentTurn.EnemyTurn)
        {
            return;
        }
        StartNextEnemy();
    }

    public void StartNextEnemy()
    {
        // If there are no more enemies to attackQueue, end the enemy turn
        if (AttackQueue.Count == 0)
        {
            StartCoroutine(EndEnemyTurn());
            return;
        }
        // Get the next enemy from the attackQueue and set it to attack
        GameObject enemy = AttackQueue[0];
        // Remove the enemy from the attackQueue
        AttackQueue.RemoveAt(0);

        // Set the enemy to attack
        enemyAI = enemy.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.SetState(EnemyStates.Attack);
        }
    }

    public void EnemyFinishedAttack()
    {
        StartNextEnemy();
    }

    private IEnumerator EndEnemyTurn()
    {
        yield return new WaitForSeconds(1f);
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy.GetComponent<Health>().IsAlive)
            {
                enemyAI = enemy.GetComponent<EnemyAI>();
                if (enemyAI != null)
                {
                    enemyAI.SetState(EnemyStates.Idle);
                }
            }
        }
         // wait for a short duration to ensure all enemies have finished their actions
        _ping.Raise(this, null); // set turn managet to end enemy turn
    }
}
