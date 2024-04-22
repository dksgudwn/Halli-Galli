using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public enum TurnEnum
{
    Host,
    Client
}

public class GameManger : MonoSingleton<GameManger>
{
    public Card BlackCardPrefab = null;
    public Card WhiteCardPrefab = null;
    public TurnEnum CurrnetTurn;
    public TurnEnum myTurn;

    private void Start()
    {
        CardManager.Instance.Setting();
        CardManager.Instance.SpawnCard(BlackCardPrefab, WhiteCardPrefab);
        CardManager.Instance.SelectRandomCard(myTurn, 4);

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
