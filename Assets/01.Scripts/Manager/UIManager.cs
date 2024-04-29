using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    public Dictionary<string, PopUpUI> popupUIDictionary = new();

    private TextUI textUI;
    private void Start()
    {
        textUI = FindObjectOfType<TextUI>();

        PopUpUI[] popUpUIs = GetComponentsInChildren<PopUpUI>();

        foreach(var popUpUI in popUpUIs)
        {
            if (!popupUIDictionary.ContainsKey(popUpUI.name))
            {
                popupUIDictionary.Add(popUpUI.name, popUpUI);
            }
            else
            {
                Debug.LogWarning($"Áßº¹ Å° : {popUpUI.name}");
            }
        }
    }
    public void ShowPanel(string uiName)
    {
        popupUIDictionary.TryGetValue(uiName, out var popupUI);

        if (popupUI != null)
        {
            popupUI.ShowUI();
        }
    }
    public void HidePanel(string uiName)
    {
        popupUIDictionary.TryGetValue(uiName, out var popupUI);

        popupUI.HideUI();
    }

    public void ShowText(string text, float deadTime)
    {
        textUI.ShowText(text, deadTime);
    }
}
