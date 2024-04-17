using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum CardType
{
    Black,
    White,
}
public struct CardInfo
{
    public CardType CardType;
    public int Number;
}

public class CardManager
{
    const int CardCount = 12;

    public static CardManager _instance;
    public static CardManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CardManager();

                if (_instance == null)
                {
                    Debug.Log("CardManager가 null입니다.");
                }
            }
            return _instance;
        }
    }

    #region 변수

    private Dictionary<CardInfo, Card> cardInfoToCardDic = new();

    public int count => cardInfoToCardDic.Count;
    #endregion

    public void Setting(Card blackCard, Card whiteCard)
    {
        for (int i = 0; i < CardCount; ++i)
        {
            CardInfo info = new CardInfo()
            {
                CardType = CardType.Black,
                Number = i
            };

            Card card = GameObject.Instantiate(blackCard);
            card.Setting(info);

            cardInfoToCardDic.Add(info, card);
        }
        for (int i = 0; i < CardCount; ++i)
        {
            CardInfo info = new CardInfo()
            {
                CardType = CardType.White,
                Number = i
            };

            Card card = GameObject.Instantiate(whiteCard);
            card.Setting(info);

            cardInfoToCardDic.Add(info, card);
        }
    }

    public List<CardInfo> Shuffle(List<CardInfo> cards)
    {
        return default;
    }
}
