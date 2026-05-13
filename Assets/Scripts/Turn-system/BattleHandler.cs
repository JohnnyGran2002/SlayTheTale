using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

public class BattleHandler : Singleton<BattleHandler>
{
    public enum CurrentTurn { PlayerTurn, EnemyTurn}
    // enum turnstatus - start gör x | end gör y 
    public enum TurnStatus {Start, Active, End}
    
    public CurrentTurn currentTurn = CurrentTurn.EnemyTurn;
    public TurnStatus turnStatus = TurnStatus.Start;
    [SerializeField] private int turnCount;
    [SerializeField] private float playerTurnTime, enemyTurnTime;
    private float _timer;
    
    public float Timer => _timer;

    private bool _waiting;

    public GameEvent turnStart, turnActive, turnEnd;

    #region Coroutine version

    private bool playerEndedTurn;
    private bool cardsCleared;

    private void Start()
    {
        turnCount = 0;
        StartCoroutine(TurnLoop());
    }

    private IEnumerator TurnLoop()
    {
        while (true)
        {
            yield return StartCoroutine(StartTurn());

            yield return StartCoroutine(ActiveTurn());

            yield return StartCoroutine(EndTurn());

            SwapTurn();
        }
    }

    private IEnumerator StartTurn()
    {
        turnStatus = TurnStatus.Start;
        turnCount++;
        Debug.Log($"START: {currentTurn}");
        turnStart.Raise(this, currentTurn);

        if (currentTurn == CurrentTurn.PlayerTurn)
        {
            cardsCleared = false;
            MusicManager.musicManager.soundIntensityParameter.Intensity = 0;
            yield return new WaitUntil(() => cardsCleared);
        }
        else if (currentTurn == CurrentTurn.EnemyTurn)
            MusicManager.musicManager.soundIntensityParameter.Intensity = 1;
        
        // Vänta en frame om UI/system behöver reagera
        yield return null;
    }

    private IEnumerator ActiveTurn()
    {
        turnStatus = TurnStatus.Active;

        Debug.Log($"ACTIVE TURN: {currentTurn}");

        turnActive.Raise(this, currentTurn);

        float timer =
            currentTurn == CurrentTurn.PlayerTurn
            ? playerTurnTime
            : enemyTurnTime;

        playerEndedTurn = false;

        while (timer > 0f && !playerEndedTurn)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator EndTurn()
    {
        turnStatus = TurnStatus.End;

        Debug.Log($"END TURN: {currentTurn}");

        turnEnd.Raise(this, currentTurn);

        // Om spelarens tur:
        // vänta tills kortsystemet säger att det är klart
        if (currentTurn == CurrentTurn.PlayerTurn)
        {
            cardsCleared = false;

            yield return new WaitUntil(() => cardsCleared);
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

    public void PlayerEndedTurn(Component sender, object data)
    {
        if (currentTurn != CurrentTurn.PlayerTurn)
            return;

        if (turnStatus != TurnStatus.Active)
            return;

        playerEndedTurn = true;
    }

    // Kallas av kortsystemet
    public void CardsCleared(Component sender, object data)
    {
        cardsCleared = true;
    }
    public void Something(Component sender, object data)
    {
        Debug.Log("Something");
    }
    public void SomethingElse(Component sender, object data)
    {
        Debug.Log("Something Else");
    }

    // Idea: "Ping" event, Turn manager listens, other scripts sends it. Use case: Whenever Turn Manager needs to "wait" for something, use Ping when done.
    public void Ping(Component sender, object data)
    {
        switch (sender)
        {
            case Teo_Test:
                switch (data)
                {
                    case "Something":
                        Debug.Log($"{sender} sent {data}");
                        break;
                    case null:
                        cardsCleared = true;
                        Debug.Log($"{sender} sent {data}");
                        break;
                    case "Something Else":
                        Debug.Log($"{sender} sent {data}");
                        break;
                }
                break;
            case CardSystem: // CardSystem Ping.Raise in Discard- & DrawAllCards
                switch (turnStatus)
                {
                    case TurnStatus.Start:
                        cardsCleared = true;
                        break;
                    case TurnStatus.Active:
                        Debug.Log($"{sender} shouldn´t send anything during this turn status");
                        break;
                    case TurnStatus.End:
                        cardsCleared = true;
                        break;
                }
                break;
            case PlayerController: // InputAction, player wants to end turn
                switch (turnStatus)
                {
                    case TurnStatus.Start:
                        print("Cant end turn yet");
                        break;
                    case TurnStatus.Active:
                        playerEndedTurn = true;
                        print("Ended turn");
                        break;
                    case TurnStatus.End:
                        print("Already ended turn");
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
        // Check sender of event and do stuff depending on sender
        // Card system says "Ping" -> im done -> turn manager listens, it's from Cardsystem, then i do this
        // Player system says "Ping" -> I want to end my turn -> turn manager listens, it's from Player, is CurrentTurn = player && turnStatus = Active? -> then -> I end turn
        // Unknown system ? -> implement a response for that system
    }

    #endregion
    #region My Code

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

}
