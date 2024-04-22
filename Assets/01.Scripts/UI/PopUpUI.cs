using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public interface IUIable
{
    public abstract void ShowUI();
    public abstract void HideUI();
}

[RequireComponent(typeof(CanvasGroup))]
public class PopUpUI : MonoBehaviour, IUIable
{
    protected CanvasGroup _panel;
    private void Awake()
    {
        _panel = GetComponent<CanvasGroup>();

        if (_panel != null)
        {
            _panel.alpha = 0;
            _panel.blocksRaycasts = false;
        }
    }
    public virtual void HideUI()
    {
        _panel.blocksRaycasts = false;
        _panel.alpha = 0;
    }

    public virtual void ShowUI()
    {
        _panel.alpha = 1;
        _panel.blocksRaycasts = true;
    }
}
