using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class StartCutSceneManager : MonoBehaviour
{
    private TextAnim textAnim;
    private bool isStartCutScene = false;

    [SerializeField] private Image fadeImg;

    [SerializeField] private Image[] images;
    [SerializeField] private TextMeshProUGUI firsttext;
    [SerializeField] private TextMeshProUGUI explaintext;

    [SerializeField] private MeshRenderer monitor;
    [SerializeField] private Material monitorOn;
    private int idx = 0;

    [SerializeField] private float coolInput = 1f;
    private float curCool = 0f;
    private void Awake()
    {
        textAnim = GetComponent<TextAnim>();
        InitCutScene();
    }
    private void InitCutScene()
    {
        curCool = 0; 
        isStartCutScene = false;
        textAnim.SetTextSpeed(0.15f);
        firsttext.SetText("");
        explaintext.SetText("");

    }
    private void Start()
    {
        StartCoroutine(StartFade());
    }

    private void Update()
    {
        curCool += Time.deltaTime;
        if(coolInput <= curCool)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (idx >= 3)
                {
                    StartCoroutine(EndFadeImg());
                    return;
                }
                if (isStartCutScene == false)
                {
                    StartCoroutine(MonitorOnStart());
                }
                else
                {
                    ShowCut();
                }
                curCool = 0;
            }
            
        }
        
    }
    IEnumerator MonitorOnStart()
    {
        isStartCutScene = true;
        textAnim.StopAnim();
        
        //firsttext.SetText("");
        textAnim.SetText(explaintext);
        textAnim.SetTextSpeed(0.05f);

        monitor.materials[1].DOColor(monitorOn.color, 1f);

        yield return new WaitForSeconds(1f);
        ShowCut();
    }
    public void ShowCut()
    {
        textAnim.StopAnim();
        textAnim.EndCheck();
        ShowFadeImage();
    }
    public void ShowFadeImage()
    {
        if(idx != 0)
        {

            images[idx - 1].DOFade(0, 1f);
        }
        
        images[idx].DOFade(1, 1f);
        idx++;
    }

    IEnumerator StartFade()
    {
        fadeImg.DOFade(0, 3f);
        yield return new WaitForSeconds(1f);
        textAnim.SetText(firsttext);
        textAnim.EndCheck();
    }
    IEnumerator EndFadeImg()
    {
        fadeImg.DOFade(1, 2f);
        images[images.Length-1].DOFade(0, 1.5f);
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("Lab");
    }
}
