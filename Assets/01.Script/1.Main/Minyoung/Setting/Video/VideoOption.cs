using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class VideoOption : MonoBehaviour
{
    FullScreenMode screenMode;
    public List<Resolution> resolutions = new List<Resolution>();

    int resolutionNum;

    public Button preBtn;
    public Button nexteBtn;

    public int resolutionsIndex = 0;

    public TextMeshProUGUI currentModeText;

    public TextMeshProUGUI CurrentResolutionTextText;

    public TMP_Dropdown resolutionDropdown;
    private void Awake()
    {
        InitUI();
    }
    void Start()
    {
        //preBtn.onClick.AddListener(() =>
        //{
        //    if (resolutionsIndex != 0)
        //    {
        //        resolutionsIndex--;
        //        string str = resolutions[resolutionsIndex].ToString();
        //        string str1 = str.Substring(0, str.LastIndexOf('@'));
        //        CurrentResolutionTextText.text = str1;
        //    }
        //});
        //nexteBtn.onClick.AddListener(() =>
        //{
        //    if (resolutionsIndex == resolutions.Count - 1)
        //    {
        //        resolutionsIndex = resolutions.Count - 1;
        //    }
        //    else
        //    {
        //        resolutionsIndex++;
        //    }
        //    string str = resolutions[resolutionsIndex].ToString();
        //    string str1 = str.Substring(0, str.LastIndexOf('@'));
        //    CurrentResolutionTextText.text = str1;
        //});
    }

    void InitUI()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRate == 144)
            {
                resolutions.Add(Screen.resolutions[i]);
            }
        }

        resolutionDropdown.options.Clear();

        int optionNum = 0;

        foreach (Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = item.width + "x" + item.height + " " + item.refreshRate + "hz";
            resolutionDropdown.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
            {
                resolutionDropdown.value = optionNum;
                optionNum++;
            }
        }

        resolutionDropdown.RefreshShownValue();
        //fullScreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    public void DropBoxOptionChange(int x)
    {
        resolutionNum = x;
    }

    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        currentModeText.text = isFull ? "FullScreen" : "Windowed";
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
        //Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }
    public void OKBtnClick()
    {
        Debug.Log("Àßºñ³¦");
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
    }
}
