using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class Card : MonoBehaviour
{
    private CardInfo myInfo;

    private TextMeshPro _number;
    private float curTrmY = 0f;
    private Tween tween;

    private Outline outline;

    private void OnEnable()
    {
        SignalHub.OnChangedGameState += OnChangedGameStateHandler;
    }
    private void OnDisable()
    {
        SignalHub.OnChangedGameState -= OnChangedGameStateHandler;
    }
    private void Awake()
    {
        outline = GetComponent<Outline>();
        _number = transform.Find("Card/Number").GetComponent<TextMeshPro>();

    }
    private void Start()
    {
        outline.OutlineMode = Outline.Mode.OutlineHidden;

        curTrmY = transform.position.y;
    }
    public void Dead()
    {
        outline.OutlineMode = Outline.Mode.OutlineHidden;

        tween.Kill();
        tween = transform.DOMoveY(curTrmY, .15f);

        Destroy(this);
    }

    public void Setting(CardInfo info)
    {
        myInfo = info;

        _number.text = info.Number == 12 ? "<size=100>-</size>" : info.Number.ToString();
    }

    private void OnMouseEnter()
    {
        if (!GameManger.Instance.IsMyTurn) return;
        if (GameManger.Instance.GameState == GameState.SelectCard) return;


        tween.Kill();
        tween = transform.DOMoveY(curTrmY + 3, .3f);
    }

    private void OnMouseExit()
    {
        tween.Kill();
        tween = transform.DOMoveY(curTrmY, .15f);
    }

    private void OnMouseDown()
    {
        if (!GameManger.Instance.IsMyTurn) return;
        if (GameManger.Instance.GameState == GameState.SelectCard) return;

        // 마우스를 클릭할 때 할 작업을 추가하세요.
        UIManager.Instance.ShowText("Select", 3f);
        UIManager.Instance.ShowPanel("InputUI");

        CardManager.Instance.CurrentCardInfo = myInfo;

        outline.OutlineMode = Outline.Mode.OutlineVisible;
        GameManger.Instance.GameState = GameState.SelectCard;
    }
    private void OnChangedGameStateHandler(GameState state)
    {
        if (state == GameState.EndCard)
        {
            outline.OutlineMode = Outline.Mode.OutlineHidden;
        }
    }

}