using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIable
{
    public abstract void ShowUI();
    public abstract void HideUI();
}

[RequireComponent(typeof(CanvasGroup))]
public class PopUpUI : MonoBehaviour, IUIable
{
    public virtual void HideUI()
    {

    }

    public virtual void ShowUI()
    {

    }
}
