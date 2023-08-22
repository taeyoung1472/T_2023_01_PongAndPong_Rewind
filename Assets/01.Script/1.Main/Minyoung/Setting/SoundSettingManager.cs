using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettingManager : MonoBehaviour
{
    #region 사운드
    public Slider audioSlider;
    public bool isMute = false;
    public float sound = 0f;

    public GameObject soundoff;
    public GameObject soundon;
    #endregion

    #region 창
    public bool isFullScreen = true;
    public GameObject screenoff;
    public GameObject screenon;
    FullScreenMode screenMode;
    #endregion

    #region fps
    [SerializeField] private List<int> fpsList = new List<int>();

    public Button preBtn;
    public Button nextBtn;

    public int index = 0;

    public TextMeshProUGUI fpsText;
    #endregion

    public void Start()
    {
        SaveDataManager.Instance.LoadSoundJSON();

        isMute = SaveDataManager.Instance.SettingValue.isMute;
        sound = SaveDataManager.Instance.SettingValue.volume;

        isFullScreen = SaveDataManager.Instance.SettingValue.isFullScreen;
        
        InitUI();
    }
    public void PreBtn()
    {
        if (index != 0)
        {
            index--;
            fpsText.text = fpsList[index].ToString();
        }
    }
    public void NextBtn()
    {
        if (index == fpsList.Count - 1)
        {
            index = fpsList.Count - 1;
        }
        else
        {
            index++;
        }
        fpsText.text = fpsList[index].ToString();
    }
    public void ApplyFPS()
    {
        fpsText.text = fpsList[index].ToString();

        Application.targetFrameRate = int.Parse(fpsText.text);

        SaveDataManager.Instance.SettingValue.fpsLimitIndex = index;

        Debug.Log(Application.targetFrameRate);
    }

    public void InitUI()
    {
        if (!isFullScreen)
        {
            screenon.SetActive(false);
            screenoff.SetActive(true);
        }
        else
        {
            screenon.SetActive(true);
            screenoff.SetActive(false);
        }


        if (isMute)
        {
            soundoff.SetActive(false);
            soundon.SetActive(true);
        }
        else
        {
            soundoff.SetActive(true);
            soundon.SetActive(false);
        }

        audioSlider.value = sound;


        SoundChange();
    }
    public void FullScreenBtn()
    {
        isFullScreen = !isFullScreen;
    }

    public void OffSound()
    {
        isMute = !isMute;
    }
    public void AudioControl()
    {
        sound = audioSlider.value;
    }
    public void SoundChange()
    {
        if (!isMute)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }

        Debug.Log(sound);
        if (sound == -40f) AudioManager.Mixer.SetFloat("Master", -80);
        else
        {
            AudioManager.Mixer.SetFloat("Master", sound);
        }
    }

    public void ChangeScreen()
    {
        screenMode = isFullScreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Screen.SetResolution(1920, 1080, screenMode);
    }

    public void SaveSetting()
    {
        //뮤트
        SoundChange();

        //음량조절
        SoundChange();

        //창 전체화면
        ChangeScreen();

        //프레임제한
        ApplyFPS();
        
        SaveDataManager.Instance.SettingValue.isFullScreen = isFullScreen;

        SaveDataManager.Instance.SettingValue.isMute = isMute;
        SaveDataManager.Instance.SettingValue.volume = sound;

        SaveDataManager.Instance.SaveSoundJSON(); 
    }

    public void ResetSetting()
    {
        isMute = true;
        isFullScreen = true;
        index = 3;
        sound = 20;

        SaveDataManager.Instance.SettingJSON(isMute, sound, isFullScreen, index);

        InitUI();

        //뮤트
        SoundChange();

        //음량조절
        SoundChange();

        //창 전체화면
        ChangeScreen();

        //프레임제한
        ApplyFPS();

        SaveDataManager.Instance.SaveSoundJSON();
    }

   
}
