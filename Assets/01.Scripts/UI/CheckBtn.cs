using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckBtn : MonoBehaviour
{
    private TMP_InputField numText;



    public void OnCheck()
    {
        int num = int.Parse(numText.text);
        CardManager.Instance.SelectCard(num);
        GameManger.Instance.GameState = GameState.EndCard;
    }
}
