public enum TurnEnum
{
    Host,
    Client
}

public class GameManger : MonoSingleton<GameManger>
{
    public TurnEnum CurrnetTurn;

    public TurnEnum TurnChange()
    {
        CurrnetTurn = (TurnEnum.Host == CurrnetTurn) ? TurnEnum.Client : TurnEnum.Host;

        return CurrnetTurn;
    }
}
