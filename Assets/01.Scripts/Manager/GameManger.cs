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
            SignalHub.OnChangedGameState.Invoke(value);
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
    /// <summary>
    /// 내 턴이 끝나면 이 함수를 호출해줌
    /// </summary>
    /// <returns></returns>
    public TurnEnum S_TurnChange()
    {
        //턴을 바꿈
        CurrnetTurn = (TurnEnum.Host == CurrnetTurn) ? TurnEnum.Client : TurnEnum.Host;

        return CurrnetTurn;
    }

}
