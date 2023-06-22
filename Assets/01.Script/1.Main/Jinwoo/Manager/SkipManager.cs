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

    private void Start()
    {
        isSkip = false;
        skipImg.fillAmount = 0;
        SkipPanelOff();
    }
    private void Update()
    {
        //if(skipImg.fillAmount >= 1f)
        //{
        //    isSkip = true;
        //    StartCoroutine(SkipCutScene());
        //    return;   
        //}

        if (Input.GetKeyDown(KeyCode.Return) )
        {
            //skipImg.fillAmount += 0.5f * Time.deltaTime;
            SkipPanelOn();

        }
        //if (Input.GetKeyUp(KeyCode.Return))
        //{
        //    skipImg.fillAmount = 0f;
        //}
    }
    private void SkipPanelOn()
    {
        Time.timeScale = 0f;
        skipPanel.SetActive(true);
        
    }
    private void SkipPanelOff()
    {
        Time.timeScale = 1f;
        skipPanel.SetActive(false);
    }
    public void Skip()
    {
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
