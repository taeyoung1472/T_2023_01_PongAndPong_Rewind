using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class StartCutSceneManager : MonoBehaviour
{
    public Volume volume;

    // Temp Glitch effect.
    private LimitlessGlitch1 glitch1;
    private Jitter jitter;

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

    [SerializeField] private AudioSource bgmAudio;
    private void Awake()
    {
        textAnim = GetComponent<TextAnim>();
        InitCutScene();
    }
    private void InitCutScene()
    {
        curCool = 0; 
        isStartCutScene = false;
        textAnim.SetTextSpeed(0.1f);
        firsttext.SetText("");
        explaintext.SetText("");

    }
    private void Start()
    {
        volume.profile.TryGet(out glitch1);
        volume.profile.TryGet(out jitter);
        jitter.active = true;
        glitch1.active = true;

        StartCoroutine(StartFade());

        
    }

    private void Update()
    {
        curCool += Time.deltaTime;
        if(coolInput <= curCool && !SkipManager.Instance.isSkipPanelOn)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (idx >= 3)
                {
                    textAnim.isSkip = true;
                    StartCoroutine(EndFadeImg());
                    return;
                }
                if (isStartCutScene == false)
                {
                    StartCoroutine(MonitorOnStart());
                }
                else
                {
                    if(textAnim.isAnim)
                    {
                        textAnim.isSkip = true;
                    }
                    else
                    {
                        ShowCut();
                    }
                }
                curCool = 0;
            }
            
        }
        
    }
    IEnumerator MonitorOnStart()
    {
        AudioManager.PlayAudio(SoundType.OnMonitor);

        jitter.enable.value = false;
        glitch1.enable.value = false;

        isStartCutScene = true;
        textAnim.StopAnim();
        
        //firsttext.SetText("");
        textAnim.SetText(explaintext);
        textAnim.SetTextSpeed(0.05f);

        monitor.materials[1].DOColor(monitorOn.color, 1f);

        yield return new WaitForSeconds(1f);
        bgmAudio.Play();
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
        fadeImg.gameObject.SetActive(true);
        fadeImg.DOFade(0, 3f);
        jitter.enable.value = true;
        glitch1.enable.value = true;
        yield return new WaitForSeconds(1f);
        AudioManager.PlayAudio(SoundType.StartCutScene);
        textAnim.SetText(firsttext);
        textAnim.EndCheck();

        
    }
    IEnumerator EndFadeImg()
    {
        bgmAudio.Stop();
        fadeImg.DOFade(1, 1.8f);
        images[images.Length-1].DOFade(0, 1.3f);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Lab Jinwoo");
        MainMenuManager.isOpend = true;
    }
}
