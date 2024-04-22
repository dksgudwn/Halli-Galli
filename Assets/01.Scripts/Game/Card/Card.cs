using DG.Tweening;
using UnityEngine;

public class Card : MonoBehaviour
{
    private bool _hi = false;
    private float curTrmY = 0f;
    private Tween tween;
    private void Start()
    {
        curTrmY = transform.position.y;
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
        // ���콺�� Ŭ���� �� �� �۾��� �߰��ϼ���.
    }
}