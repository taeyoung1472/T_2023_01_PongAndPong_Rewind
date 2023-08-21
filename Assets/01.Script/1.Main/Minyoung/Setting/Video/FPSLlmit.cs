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

    FullScreenMode screenMode;


    public void Start()
    {

        fpsText.text = SaveDataManager.Instance.SettingValue.fpsLimitIndex.ToString();
        Application.targetFrameRate = int.Parse(fpsText.text);



        preBtn.onClick.AddListener(() =>
        {
            if (index != 0)
            {
                index--;
                fpsText.text = fpsList[index].ToString();
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
        });
    }
    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Screen.SetResolution(1920, 1080, screenMode);
    }

    public void ApplyFPS()
    {
        Application.targetFrameRate = int.Parse(fpsText.text);

        SaveDataManager.Instance.SettingValue.fpsLimitIndex = int.Parse(fpsText.text); 
        
        Debug.Log(Application.targetFrameRate);
    }

}
