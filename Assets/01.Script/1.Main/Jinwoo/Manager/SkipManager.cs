 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SkipManager : MonoSingleTon<SkipManager>
{
    [SerializeField] private GameObject skipPanel;
    [SerializeField] private Image skipImg;
    public bool isSkip = false;
    public bool isSkipPanelOn = false;

    private void Start()
    {
        isSkip = false;
        isSkipPanelOn = false;
        skipImg.fillAmount = 0;
        SkipPanelOff();
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return) )
        {
            SkipPanelOn();

        }
        if (isSkipPanelOn)
        {
            if(Input.GetKeyDown(KeyCode.Escape) )
            {
                SkipPanelOff();    
            }
        }
    }
    private void SkipPanelOn()
    {
        isSkipPanelOn = true;
        Time.timeScale = 0f;
        skipPanel.SetActive(true);
        
    }
    private void SkipPanelOff()
    {
        isSkipPanelOn = false;
        Time.timeScale = 1f;
        skipPanel.SetActive(false);
    }
    public void Skip()
    {
        AudioManager.PlayAudio(SoundType.SkipSound);
        isSkip = true;
        SkipPanelOff();
        StartCoroutine(SkipCutScene());
    }
    public void NoSkip()
    {
        SkipPanelOff();
    }
    IEnumerator SkipCutScene()
    {
        FadeInOutManager.Instance.FadeIn(2f);
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("NewLab");
    }
}
