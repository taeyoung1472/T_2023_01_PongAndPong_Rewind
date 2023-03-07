using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleTon<UIManager>
{
    [Header("[Clock]")]
    [SerializeField] private Image clockFill;
    [SerializeField] private TextMeshProUGUI clockTimeText;
    [SerializeField] private Color[] clockColorArray;

    //private int totalTIme { get { return RewindManager.Instance.CurStageRecordCount; } }
    private int totalTIme { get { return StageManager.Instance.curArea.PlayTime; } }

    public void Init()
    {
        //RewindManager.Instance.OnTimeChanging += OnTimeChange;
        
    }

    public void OnTimeChange(int time)
    {
        clockFill.fillAmount = time / (float)(totalTIme - 1);
        clockTimeText.text = $"{time}";

        int clockTime = totalTIme / clockColorArray.Length;
        int tempTime = 0;
        for (int i = 0; i < clockColorArray.Length; i++)
        {
            if (time >= tempTime)
            {
                clockFill.color = Color.Lerp(clockColorArray[i], clockColorArray[Mathf.Clamp(i + 1, 0, clockColorArray.Length - 1)], (float)(time % clockTime) / (float)clockTime);
            }
            tempTime += clockTime;
        }
    }
}
