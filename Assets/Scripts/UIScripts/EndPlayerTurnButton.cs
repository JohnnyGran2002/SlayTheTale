using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPlayerTurnButton : MonoBehaviour
{
    public GameEvent PlayerTurnEnded;
    public GameEvent EnemyTurnStarted;

    public void EndPlayerTurn()
    {
        PlayerTurnEnded.Raise(this, null);

        StartCoroutine(StartEnemyTurn());
    }

    private IEnumerator StartEnemyTurn()
    {
        yield return new WaitForSeconds(2);

        EnemyTurnStarted.Raise(this, null);
    }
}
