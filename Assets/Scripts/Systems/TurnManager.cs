using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnManager : Singleton<TurnManager>
{
    #region New version
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

    #region Coroutine version

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
        Debug.Log($"START: {currentTurn}");
        turnStart.Raise(this, currentTurn);

        if (currentTurn == CurrentTurn.PlayerTurn)
        {
            _cardsCleared = false;
            MusicManager.musicManager.soundIntensityParameter.Intensity = 0;
            yield return new WaitUntil(() => _cardsCleared);
        }
        else if (currentTurn == CurrentTurn.EnemyTurn)
        {
            _waitingForEnemy = false;
            MusicManager.musicManager.soundIntensityParameter.Intensity = 1;
            yield return new WaitUntil(() => _waitingForEnemy);
        }
        
        yield return null;
    }

    private IEnumerator ActiveTurn() // What will happen at an Active state of a turn
    {
        turnStatus = TurnStatus.Active;

        Debug.Log($"ACTIVE TURN: {currentTurn}");

        turnActive.Raise(this, currentTurn);

        float timer =
            currentTurn == CurrentTurn.PlayerTurn
            ? playerTurnTime
            : enemyTurnTime;

        _playerEndedTurn = false;
        if (currentTurn == CurrentTurn.PlayerTurn)
        {
            while (timer > 0f && !_playerEndedTurn)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
        }
        else if (currentTurn == CurrentTurn.EnemyTurn)
        {
            _waitingForEnemy = false;
            yield return new WaitUntil(() => _waitingForEnemy);
        }
    }

    private IEnumerator EndTurn() // What will happen at the End state of a turn
    {
        turnStatus = TurnStatus.End;

        Debug.Log($"END TURN: {currentTurn}");

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

        Debug.Log("Turn cleanup complete");
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
                            Debug.Log($"{sender} shouldn´t send anything during this turn status");
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
            /* Switch template
              switch (turnStatus)
                {
                    case TurnStatus.Start:
                        break;
                    case TurnStatus.Active:
                        break;
                    case TurnStatus.End:
                        break;
                }
             */
        }
    }

    #endregion
    #region First draft

    #if false
    
    private void OnEnable()
    {
        turnStatus = TurnStatus.Start;
        OnStartTurn();
    }
    private void Update()
    {
        if (turnStatus == TurnStatus.Active)
        {
            if (_timer <= 0.0f)
            {
                OnEndTurn();
            }
            _timer -= Time.deltaTime;
        }
    }

    private void TimeManager()
    {
        _timer = currentTurn == CurrentTurn.PlayerTurn ? playerTurnTime : enemyTurnTime;
    }

    private void ChangeTurnStatus(CurrentTurn current)
    {
        if (turnStatus != TurnStatus.End) return;
        switch (current)
        {
            case CurrentTurn.PlayerTurn:
                current = CurrentTurn.EnemyTurn;
                break;
            case CurrentTurn.EnemyTurn:
                current = CurrentTurn.PlayerTurn;
                break;
        }
        currentTurn = current;
    }
    // Events skickas med enum som säger vems turn det är,
    // samma event kan användas av olika lyssnare
    // lyssnaren gör valet själv om den ska agera baserat på enum
    public void OnStartTurn()
    {
        turnStatus = TurnStatus.Start;
        turnStart.Raise(this, currentTurn);
        TimeManager();
        Debug.Log($"OnStartTurn - Current turn: {currentTurn} TurnStatus: {turnStatus}");
        switch (currentTurn)
        {
            case CurrentTurn.PlayerTurn:
                // do this
                Debug.Log("Its player turn so im doing player stuff");
            break;
            case CurrentTurn.EnemyTurn:
                // do that
                Debug.Log("Its enemy turn so im doing enemy stuff");
            break;
        }
        //last thing
        OnActiveTurn();
    }

    public void OnActiveTurn()
    {
        turnStatus = TurnStatus.Active;
        turnActive.Raise(this, currentTurn);
        Debug.Log($"OnActiveTurn - Current turn: {currentTurn} TurnStatus: {turnStatus}");
        switch (currentTurn)
        {
            case CurrentTurn.PlayerTurn:
                // do this
                Debug.Log("Its player turn so im doing player stuff");
                break;
            case CurrentTurn.EnemyTurn:
                // do that
                Debug.Log("Its enemy turn so im doing enemy stuff");
                break;
        }
        //last thing
    }

    // Lyssnar på när spelaren vill avsluta sin turn, men bara om det är spelarens turn samt den är aktiv
    public void PlayerEndedTurn(Component sender, object data)
    {
        if (currentTurn != CurrentTurn.PlayerTurn && turnStatus != TurnStatus.Active) return;
        OnEndTurn();
    }
    public void OnEndTurn()
    {
        turnStatus = TurnStatus.End;
        turnEnd.Raise(this, currentTurn);
        Debug.Log($"OnEndTurn - Current turn: {currentTurn}  TurnStatus: {turnStatus}");
        switch (currentTurn)
        {
            case CurrentTurn.PlayerTurn:
                // do this
                Debug.Log("Its player turn so im doing player stuff");
                break;
            case CurrentTurn.EnemyTurn:
                // do that
                Debug.Log("Its enemy turn so im doing enemy stuff");
                break;
        }
        //last thing
        ChangeTurnStatus(currentTurn);
        OnStartTurn();
    }
    
#endif

    #endregion


    #endregion

#if false
    #region Old version

    [SerializeField] private int _currentTurn;
    public enum turnStatus { playerTurn, enemyTurn }
    public turnStatus currentTurnStatus = turnStatus.enemyTurn;
    public GameEvent playerTurnStarted, playerTurnEnded, enemyTurnStarted, enemyTurnEnded;

    // Start is called before the first frame update
    void Start()
    {
        _currentTurn = 0;
        enemyTurnStarted.Raise(this, null);
    }

    public void OnEndTurn(InputAction.CallbackContext context)
    {
        if (currentTurnStatus == turnStatus.playerTurn && context.performed)
        {
            playerTurnEnded.Raise(this, null);
            currentTurnStatus = turnStatus.enemyTurn;
        }
    }

    public void StartPlayerTurn(Component sender, object data)
    {
        _currentTurn++;
        playerTurnStarted.Raise(this, null);
        currentTurnStatus = turnStatus.playerTurn;
        Debug.Log("PlayerTurn");
        MusicManager.musicManager.soundIntensityParameter.Intensity = 0;
    }

    public void StartEnemyTurn(Component sender, object data)
    {
        enemyTurnStarted.Raise(this, null);
        currentTurnStatus = turnStatus.enemyTurn;
        Debug.Log("EnemyTurn");
        MusicManager.musicManager.soundIntensityParameter.Intensity = 1;
    }

    #endregion
#endif
   
}
