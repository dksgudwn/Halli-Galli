using DummyClient;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputUI : PopUpUI
{

    [SerializeField] private TMP_InputField numText;
    protected override void Awake()
    {
        base.Awake();
    }
    public override void HideUI()
    {
        base.HideUI();
    }

    public override void ShowUI()
    {
        base.ShowUI();
    }

    public void OnCheck()
    {
        int num = 0;

        if (int.TryParse(numText.text, out num))
        {
            C_CheckCard card = new C_CheckCard();
            //card.SelectIdx = ;
            card.Answer = num;
            CardManager.Instance.SelectCard(num);
        }
        GameManger.Instance.GameState = GameState.EndCard;
    }

    public void OnCancel()
    {
        GameManger.Instance.GameState = GameState.EndCard;
    }
}
