using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnManager : Singleton<TurnManager>
{
    public enum CurrentTurn { PlayerTurn, EnemyTurn}
    // enum turnstatus - start gör x | end gör y 
    public enum TurnStatus {Start, Active, End}
    
    public CurrentTurn currentTurn;
    public TurnStatus turnStatus = TurnStatus.Start;
    
    [SerializeField] private float playerTurnTime, enemyTurnTime;
    private float _timer;
    private int _turnCount;
    
    public float Timer => _timer;
    public int TurnCount => _turnCount;

    private bool _waiting;

    public GameEvent turnStart, turnActive, turnEnd;

    private bool _playerEndedTurn;
    private bool _cardsCleared;
    private bool _waitingForEnemy;

    private void Start()
    {
        _turnCount = 0;
        StartCoroutine(TurnLoop());
    }

    private IEnumerator TurnLoop() // Infinite loop as of right now
    {
        while (true)
        {
            yield return StartCoroutine(StartTurn());

            yield return StartCoroutine(ActiveTurn());

            yield return StartCoroutine(EndTurn());

            SwapTurn();
        }
    }

    private IEnumerator StartTurn() // What will happen at the Start state of a turn
    {
        turnStatus = TurnStatus.Start;
        _turnCount++;
        turnStart.Raise(this, currentTurn);

        if (currentTurn == CurrentTurn.PlayerTurn)
        {
            _cardsCleared = false;
            MusicManager.musicManager.soundIntensityParameter.Intensity = 0;
            yield return new WaitUntil(() => _cardsCleared);
        }
        else if (currentTurn == CurrentTurn.EnemyTurn)
        {
            MusicManager.musicManager.soundIntensityParameter.Intensity = 1;
            yield return new WaitUntil(() => _waitingForEnemy);
        }
        _waitingForEnemy = false;
        yield return null;
    }

    private IEnumerator ActiveTurn() // What will happen at an Active state of a turn
    {
        turnStatus = TurnStatus.Active;

        turnActive.Raise(this, currentTurn);
        _playerEndedTurn = false;

        float timer =
            currentTurn == CurrentTurn.PlayerTurn
            ? playerTurnTime
            : enemyTurnTime;

        if (currentTurn == CurrentTurn.PlayerTurn)
        {
            yield return new WaitUntil(() => _playerEndedTurn);
            // while (timer > 0f && !_playerEndedTurn)
            // {
            //     timer -= Time.deltaTime;
            //     yield return null;
            // }
        }
        else if (currentTurn == CurrentTurn.EnemyTurn)
        {
            yield return new WaitUntil(() => _waitingForEnemy);
        }
        _waitingForEnemy = false;
    }

    private IEnumerator EndTurn() // What will happen at the End state of a turn
    {
        turnStatus = TurnStatus.End;

        turnEnd.Raise(this, currentTurn);
        
        if (currentTurn == CurrentTurn.PlayerTurn)
        {
            _cardsCleared = false;

            yield return new WaitUntil(() => _cardsCleared);
        }
        else if (currentTurn == CurrentTurn.EnemyTurn)
        {
            PlaySFX playSfx = GetComponent<PlaySFX>();
            playSfx.Play();
        }

    }

    private void SwapTurn()
    {
        currentTurn =
            currentTurn == CurrentTurn.PlayerTurn
            ? CurrentTurn.EnemyTurn
            : CurrentTurn.PlayerTurn;
    }

    // Remove?
    public void PlayerEndedTurn(Component sender, object data)
    {
        if (currentTurn != CurrentTurn.PlayerTurn)
            return;

        if (turnStatus != TurnStatus.Active)
            return;

        _playerEndedTurn = true;
    }
    
    // Idea: "Ping" event, Turn manager listens, other scripts sends it.
    // Use case: Whenever Turn Manager needs to "wait" for something, use Ping when done.
    // Check sender of event and do stuff depending on sender
    // Card system says "Ping" -> im done -> turn manager listens, turn status is End, its finished discarding cards
    // Player system says "Ping" -> I want to end my turn -> turn manager listens, it's from Player,
    // is CurrentTurn = player && turnStatus = Active? -> then I end turn
    // Unknown system ? -> implement a response for that system, use switch template down below
    public void Ping(Component sender, object data)
    {
        switch (sender)
        {
            case CardSystem: // CardSystem Ping.Raise in Discard- & DrawAllCards
                if (currentTurn == CurrentTurn.PlayerTurn)
                    switch (turnStatus)
                    {
                        case TurnStatus.Start:
                            _cardsCleared = true;
                            break;
                        case TurnStatus.Active:
                            break;
                        case TurnStatus.End:
                            _cardsCleared = true;
                            break;
                    }
                break;
            case PlayerController: // InputAction, player wants to end turn
                if (currentTurn == CurrentTurn.PlayerTurn)
                    switch (turnStatus)
                    {
                        case TurnStatus.Start:
                            print("Cant end turn yet");
                            break;
                        case TurnStatus.Active:
                            _playerEndedTurn = true;
                            print("Ended turn");
                            break;
                        case TurnStatus.End:
                            print("Already ended turn");
                            break;
                    }
                break;
            case EnemyAttackCoordinator:
                if (currentTurn == CurrentTurn.EnemyTurn)
                    switch (turnStatus)
                    {
                        case TurnStatus.Start:
                            _waitingForEnemy = true;
                            break;
                        case TurnStatus.Active:
                            _waitingForEnemy = true;
                            break;
                        case TurnStatus.End:
                            break;
                    }
                break;
        }
    }
}