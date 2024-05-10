using UnityEngine;

public enum TurnEnum
{
    Host,
    Client
}
public enum GameState
{

    Running,
    SelectCard,
    EndCard,
    Leave,
}

public class GameManger : MonoSingleton<GameManger>
{
    public Card BlackCardPrefab = null;
    public Card WhiteCardPrefab = null;
    public TurnEnum CurrnetTurn;
    public TurnEnum myTurn;

    private GameState _gameState = GameState.Running;
    public GameState GameState
    {
        get
        {
            return _gameState;
        }
        set
        {
            SignalHub.OnChangedGameState?.Invoke(value);
            _gameState = value;
        }
    }

    public bool IsMyTurn => CurrnetTurn == myTurn;
    private void Start()
    {
        CardManager.Instance.Setting();
        CardManager.Instance.SpawnCard(BlackCardPrefab, WhiteCardPrefab);
        CardManager.Instance.GetRandomCard(TurnEnum.Client, 4);
        CardManager.Instance.GetRandomCard(TurnEnum.Host, 4);

        //CardManager.Instance.SelectRandomCard(TurnEnum.Client, 4);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            S_TurnChange();
        }
    }

    public TurnEnum S_TurnChange()
    {
        //≈œ¿ª πŸ≤ﬁ
        CurrnetTurn = (TurnEnum.Host == CurrnetTurn) ? TurnEnum.Client : TurnEnum.Host;
        CardManager.Instance.GetRandomCard(CurrnetTurn, 1);
        return CurrnetTurn;
    }

}
