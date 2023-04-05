using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FPSLlmit : MonoBehaviour
{
    [SerializeField] private List<int> fpsList = new List<int>();

    public Button preBtn;
    public Button nextBtn;

    public int index = 0;

    public TextMeshProUGUI fpsText;

    private void Awake()
    {
        Application.targetFrameRate = fpsList[index];
    }

    public void Start()
    {
        preBtn.onClick.AddListener(() =>
        {
            if (index != 0)
            {
                index--;
                fpsText.text = fpsList[index].ToString();
                ApplyFPS();
            }
        });
        nextBtn.onClick.AddListener(() =>
        {
            if (index == fpsList.Count - 1)
            {
                index = fpsList.Count -1;
            }
            else
            {
                index++;
            }
            fpsText.text = fpsList[index].ToString();
            ApplyFPS();
        });
    }
    public void ApplyFPS()
    {
        Application.targetFrameRate = int.Parse(fpsText.text);
        Debug.Log(Application.targetFrameRate);
    }

}
