using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("[Clock]")]
    [SerializeField] private Image clockFill;
    [SerializeField] private TextMeshProUGUI clockTimeText;

    private int totalTIme;

    public void Start()
    {
        RewindManager.Instance.OnTimeChanging += OnTimeChange;
        totalTIme = RewindManager.Instance.TotalRecordCount;
    }

    private void OnTimeChange(int time)
    {
        clockFill.fillAmount = time / (float)(totalTIme - 1);
        clockTimeText.text = $"{time * RewindManager.Instance.RecordeTurm:0}";
    }
}
