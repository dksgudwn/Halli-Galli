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

    public List<CardInfo> myCards = new(); //나의 카드
    public List<CardInfo> yourCards = new(); //상대방 카드
    public List<CardInfo> remainingCards = new(); // 남은 카드

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

    private void Start()
    {
        CardManager.Instance.Setting(BlackCardPrefab, WhiteCardPrefab);

        Debug.Log(CardManager.Instance.count);
    }
}
