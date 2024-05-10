using DummyClient;
using System;
using System.Collections.Generic;
using UnityEngine;


public enum CardType
{
    Black,
    White,
}
public class CardInfo : IComparable<CardInfo>
{
    public int CardType;
    public int Number;
    public int ownerId;
    public int CompareTo(CardInfo other)
    {
        int R_num = this.Number.CompareTo(other.Number);
        if (R_num != 0) return R_num;

        int R_type = this.CardType.CompareTo(other.CardType);
        return R_type;
    }
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
                    Debug.Log("CardManager�� null�Դϴ�.");
                }
            }
            return _instance;
        }
    }


    #region ����

    private float distance = 8f;

    private Dictionary<CardInfo, Card> cardInfoToCardDic = new();
    public int count => cardInfoToCardDic.Count;

    public List<CardInfo> clientCards = new(); //���� ī��
    public List<CardInfo> hostCards = new(); //���� ī��
    public List<CardInfo> remainingCards = new(); // ���� ī��

    public Transform ClientCardParent = null;
    public Transform HostCardParent = null;

    public CardInfo CurrentCardInfo;
    #endregion
    public void Setting()
    {
        HostCardParent = GameObject.Find("HostCard").transform;
        ClientCardParent = GameObject.Find("ClientCard").transform;
    }

    public void SpawnCard(Card blackCard, Card whiteCard)
    {
        for (int i = 0; i < CardCount; ++i)
        {
            CardInfo info = new CardInfo()
            {
                CardType = (int)CardType.Black,
                Number = i
            };

            Card card = GameObject.Instantiate(blackCard);
            card.Setting(info);
            card.gameObject.SetActive(false);


            cardInfoToCardDic.Add(info, card);
            remainingCards.Add(info);
        }
        for (int i = 0; i < CardCount; ++i)
        {
            CardInfo info = new CardInfo()
            {
                CardType = (int)CardType.White,
                Number = i
            };

            Card card = GameObject.Instantiate(whiteCard);
            card.Setting(info);
            card.gameObject.SetActive(false);

            cardInfoToCardDic.Add(info, card);
            remainingCards.Add(info);
        }
    }
    public void SelectCard(int number)
    {
        if (CurrentCardInfo.Number == number)
        {
            UIManager.Instance.ShowText("Good~~~", 3f);

            if (cardInfoToCardDic.TryGetValue(CurrentCardInfo, out var value))
            {
                if (GameManger.Instance.CurrnetTurn == TurnEnum.Host)
                {
                    clientCards.Remove(CurrentCardInfo);
                }
                else
                {
                    hostCards.Remove(CurrentCardInfo);
                }
            }
            cardInfoToCardDic.Remove(CurrentCardInfo);
            value.Dead();
        }
        else
        {
            UIManager.Instance.ShowText("hahaha~ You Are Wrong", 3f);
        }
    }


    public void GetRandomCard(TurnEnum type, int count)
    {
        if (remainingCards.Count <= 0) return;
        remainingCards.Shuffle();

        SetCard(type, count);
        SetPositions(clientCards);
        SetPositions(hostCards);
    }
    public void SetCard(TurnEnum type, int count)
    {
        for (int i = 0; i < count; ++i)
        {
            var selectCard = remainingCards[i];
            selectCard.ownerId = (int)type + 1;

            var card = selectCard;

            C_RandomCard cardPacket = new C_RandomCard();
            cardPacket.Num = card.Number;
            cardPacket.Color = card.CardType;
            if (DaVinciCode.Instance.localTurn == DaVinciCode.Turn.Own)          //������ ���
                cardPacket.destinationId = (int)DaVinciCode.Turn.Opponent;
            else
                cardPacket.destinationId = (int)DaVinciCode.Turn.Own;
            Debug.Log(cardPacket.destinationId);

            cardInfoToCardDic.TryGetValue(card, out var value);
            if (type == TurnEnum.Client)
            {
                clientCards.Add(card);
                value.transform.parent = ClientCardParent;
            }
            else if (type == TurnEnum.Host)
            {
                hostCards.Add(card);
                value.transform.parent = HostCardParent;
            }

            value.gameObject.SetActive(true);
        }

        remainingCards.RemoveRange(0, count);

    }
    private void SetPositions(List<CardInfo> cards)
    {
        cards.Sort();

        int i = 1;
        foreach (var item in cards)
        {
            float xPos = i * distance;

            Debug.Log(xPos);
            if (cardInfoToCardDic.TryGetValue(item, out var card))
            {
                card.transform.localPosition = new Vector3(xPos, 0, 0);
                card.transform.localEulerAngles = new Vector3(0, 180, 0);
            }
            i++;
        }
    }
}
