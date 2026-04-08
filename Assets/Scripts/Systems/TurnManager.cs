using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    [SerializeField] private int _currentTurn;
    public enum turnStatus {playerTurn, enemyTurn}
    public turnStatus currentTurnStatus = turnStatus.playerTurn;
    public GameEvent playerTurnStarted, playerTurnEnded, enemyTurnStarted, enemyTurnEnded;

    // Start is called before the first frame update
    void Start()
    {
        _currentTurn = 0;
        playerTurnStarted.Raise(this, null);
    }

    public void StartPlayerTurn(Component sender, object data)
    {
        _currentTurn++;
        currentTurnStatus = turnStatus.playerTurn;
    }

    public void StartEnemyTurn(Component sender, object data)
    {
        currentTurnStatus = turnStatus.enemyTurn;
    }
}
