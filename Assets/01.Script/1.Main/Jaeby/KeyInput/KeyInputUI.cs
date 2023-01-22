using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyInputUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI funtionText;
    [SerializeField] private TextMeshProUGUI keyText;

    public void DisplayData(InputType inputType, KeyCode keyCode)
    {
        funtionText.SetText($"±â´É : {inputType}");
        keyText.SetText(keyCode.ToString());
    }
}
