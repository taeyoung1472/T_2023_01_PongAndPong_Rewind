using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private float textDelay;

    public void DisplayText(string s)
    {
        StartCoroutine(WriteText(s));
    }

    IEnumerator WriteText(string text)
    {
        tmp.text = "";

        int curIndex = 0;
        char targetChar = text[curIndex];
        while(true) 
        {
            targetChar = text[curIndex];
            
            tmp.text += targetChar;

            curIndex++;

            if(curIndex == text.Length)
            {
                break;
            }

            yield return new WaitForSeconds(textDelay);
        }
    }
}
