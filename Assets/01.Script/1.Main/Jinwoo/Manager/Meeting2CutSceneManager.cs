using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.Playables;

public class Meeting2CutSceneManager : MonoSingleTon<Meeting2CutSceneManager>
{

    private bool isMeetingStart = false;

    private float spacebarCoolTime = 1.5f;
    private float curCool = 0;

    [SerializeField] private int autoTalkingIndex = 1;

    [SerializeField] private TextAnim[] npcTexts;

    [SerializeField] private CinemachineVirtualCamera[] npcCam;

    [SerializeField] private CinemachineVirtualCamera meetingCam;

    [SerializeField] private PlayableDirector dayCutscene;
    private IEnumerator Start()
    {
        AllClearText();

        autoTalkingIndex = 1;

        yield return new WaitForSeconds(1.5f);
        isMeetingStart = true;
        CheckAutoTalkSpeechBubble();
    }
    private void Update()
    {
        if (isMeetingStart)
        {
            curCool += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space) && spacebarCoolTime <= curCool)
            {
                CheckAutoTalkSpeechBubble();
                curCool = 0;
            }
        }
    }
    public void CheckAutoTalkSpeechBubble()
    {
        switch (autoTalkingIndex)
        {
            case 1:
                ShowSpeechBubble(1);
                break;
            case 2:
                ShowSpeechBubble(1);
                break;
            case 3:
                ShowSpeechBubble(2);
                break;
            case 4:
                ShowSpeechBubble(5);
                break;
            case 5:
                ShowSpeechBubble(5);
                break;
            case 6:
                ShowSpeechBubble(4);
                break;
            case 7:
                ShowSpeechBubble(4);
                break;
            case 8:
                ShowSpeechBubble(4);
                break;
            case 9:
                ShowSpeechBubble(3);
                break;
            case 10:
                ShowSpeechBubble(1);
                break;
            case 11:
                ShowSpeechBubble(1);
                break;
            case 12:
                ShowSpeechBubble(1);
                break;
            case 13:
                ShowSpeechBubble(2);
                break;
            case 14:
                ShowSpeechBubble(4);
                break;
            case 15:
                ShowSpeechBubble(6);
                break;
            case 16:
                ShowSpeechBubble(6);
                break;
            case 17:
                ShowSpeechBubble(6);
                break;
            case 18:
                ShowSpeechBubble(6);
                break;
            case 19:
                ShowSpeechBubble(2);
                break;
            case 20:
                StartCoroutine(EndTalk());
                break;
            default:
                break;
        }
        autoTalkingIndex++;
    }

    private void AllClearText()
    {
        foreach (var text in npcTexts)
        {
            text.ClearText();
            text.gameObject.SetActive(false);
        }
    }
    private IEnumerator EndTalk()
    {
        AllClearText();

        isMeetingStart = false;
        FadeInOutManager.Instance.FadeIn(2f);
        yield return new WaitForSeconds(2.2f);
        dayCutscene.Play();

    }

    public void ShowSpeechBubble(int npcNum)
    {
        for (int i = 0; i < npcCam.Length; i++)
        {
            if (i == (npcNum - 1))
            {
                if (!npcTexts[i].gameObject.activeSelf)
                    npcTexts[i].gameObject.SetActive(true);
                npcTexts[i].StopAnim();
                npcTexts[i].EndCheck();

                npcCam[i].Priority +=2;
            }
            else
            {
                npcTexts[i].ClearText();
                npcTexts[i].gameObject.SetActive(false);
                npcCam[i].Priority = 10;
            }
        }
    }

}
