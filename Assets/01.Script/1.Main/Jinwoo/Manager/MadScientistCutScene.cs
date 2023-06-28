using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MadScientistCutScene : MonoSingleTon<MadScientistCutScene>
{
    [SerializeField] private CinemachineVirtualCamera vcam;

    private bool isMadScientistStart = false;

    [SerializeField]private float spacebarCoolTime = .5f;
    private float curCool = 0;

    [SerializeField] private int autoTalkingIndex = 1;

    [SerializeField] private TextAnim madScientistText;
    [SerializeField] private Animator madAnim;
    private IEnumerator Start()
    {
        autoTalkingIndex = 1;
        FadeInOutManager.Instance.FadeOut(5f);
        vcam.Priority = 12;

        madScientistText.ClearText();
        madScientistText.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);
        isMadScientistStart = true;

        CheckAutoTalkSpeechBubble();

        JinwooVolumeManager.Instance.StartCinematicBars();

        JinwooVolumeManager.Instance.EnableGlitch();
    }
    private void Update()
    {
        if (isMadScientistStart)
        {
            curCool += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space) && spacebarCoolTime <= curCool)
            {
                if (madScientistText.isAnim)
                {
                    madScientistText.isSkip = true;
                }
                else
                {
                    CheckAutoTalkSpeechBubble();
                }
                curCool = 0;
            }
        }
    }
    public IEnumerator EndMadAloneTalk()
    {
        madScientistText.ClearText();
        madScientistText.gameObject.SetActive(false);
        isMadScientistStart = false;

        FadeInOutManager.Instance.FadeIn(2f);
        JinwooVolumeManager.Instance.DisableGlitch();
        yield return new WaitForSeconds(2f);
        JinwooVolumeManager.Instance.EndCinmaticBars();
        FadeInOutManager.Instance.FadeOut(2f);

        MonitorNPCTalk.Instance.StartMeeting();
        this.gameObject.SetActive(false);
    }
    public void CheckAutoTalkSpeechBubble()
    {
        
        if (autoTalkingIndex >= 8)
        {
            StartCoroutine(EndMadAloneTalk());
        }
        else
        {
            if (autoTalkingIndex == 2)
            {
                madAnim.Play("Action1");
            }
            else if (autoTalkingIndex == 4)
            {
                madAnim.Play("Action2");
            }
            else if (autoTalkingIndex == 6)
            {
                madAnim.Play("Action1");
            }
            ShowSpeechBubble();
            autoTalkingIndex++;
        }
    }
    public void ShowSpeechBubble()
    {
        if (!madScientistText.gameObject.activeSelf)
            madScientistText.gameObject.SetActive(true);
        madScientistText.StopAnim();
        madScientistText.EndCheck();
    }
}
