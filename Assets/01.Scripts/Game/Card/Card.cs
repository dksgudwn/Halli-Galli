using DG.Tweening;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    private CardInfo myInfo;

    private TextMeshPro _number;
    private float curTrmY = 0f;
    private Tween tween;

    private void Awake()
    {
        _number = transform.Find("Card/Number").GetComponent<TextMeshPro>();
    }
    private void Start()
    {
        curTrmY = transform.position.y;
    }

    public void Setting(CardInfo info)
    {
        myInfo = info;

        _number.text = info.Number.ToString();
    }

    private void OnMouseEnter()
    {
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
        // 마우스를 클릭할 때 할 작업을 추가하세요.
    }
}