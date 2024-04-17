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

    public List<CardInfo> myCards = new(); //���� ī��
    public List<CardInfo> yourCards = new(); //���� ī��
    public List<CardInfo> remainingCards = new(); // ���� ī��

    /// <summary>
    /// �� ���� ������ �� �Լ��� ȣ������
    /// </summary>
    /// <returns></returns>
    public TurnEnum S_TurnChange()
    {
        //���� �ٲ�
        CurrnetTurn = (TurnEnum.Host == CurrnetTurn) ? TurnEnum.Client : TurnEnum.Host;

        return CurrnetTurn;
    }

    private void Start()
    {
        CardManager.Instance.Setting(BlackCardPrefab, WhiteCardPrefab);

        Debug.Log(CardManager.Instance.count);
    }
}
