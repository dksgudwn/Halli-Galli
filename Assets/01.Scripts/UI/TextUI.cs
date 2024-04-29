using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextUI : MonoBehaviour
{
    private TextMeshProUGUI _txt;
    private Coroutine _coroutine;

    private void Awake()
    {
        _txt = GetComponent<TextMeshProUGUI>();
    }
    public void ShowText(string text, float deadTime)
    {
        _txt.text = text;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(DeadCorou(deadTime));
    }

    private IEnumerator DeadCorou(float deadTime)
    {
        yield return new WaitForSeconds(deadTime);

        _txt.text = "";
    }
}
