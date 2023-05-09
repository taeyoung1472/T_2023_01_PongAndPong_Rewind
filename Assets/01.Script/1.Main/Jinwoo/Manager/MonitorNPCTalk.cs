using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.Playables;

public class MonitorNPCTalk : MonoSingleTon<MonitorNPCTalk>
{
    private bool isMeetingStart = false;

    private float spacebarCoolTime = 1.5f;
    private float curCool = 0;

    [SerializeField] private int autoTalkingIndex = 1;

    [SerializeField] private TextAnim[] npcTexts;

    [SerializeField] private CinemachineVirtualCamera meetingCam;

    private void Start()
    {
        AllClearText();

        //isMeetingStart = true;
        autoTalkingIndex = 1;
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
    public void StartMeeting()
    {
        meetingCam.enabled = true;
        meetingCam.Priority = 15;

        AllClearText();
        autoTalkingIndex = 1;

        isMeetingStart = true;
        CheckAutoTalkSpeechBubble();
    }
    public void CheckAutoTalkSpeechBubble()
    {
        switch (autoTalkingIndex)
        {
            case 1:
                ShowSpeechBubble(1);
                break;
            case 2:
                ShowSpeechBubble(2);
                break;
            case 3:
                ShowSpeechBubble(1);
                break;
            case 4:
                ShowSpeechBubble(1);
                break;
            case 5:
                ShowSpeechBubble(3);
                break;
            case 6:
                ShowSpeechBubble(2);
                break;
            case 7:
                ShowSpeechBubble(2);
                break;
            case 8:
                ShowSpeechBubble(1);
                break;
            case 9:
                ShowSpeechBubble(3);
                break;
            case 10:
                ShowSpeechBubble(1);
                break;
            case 11:
                ShowSpeechBubble(2);
                break;
            case 12:
                ShowSpeechBubble(3);
                break;
            case 13:
                EndTalk();
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
    private void EndTalk()
    {
        AllClearText();

        isMeetingStart = false;
        StartCoroutine(NextCutScene());

    }
    public IEnumerator NextCutScene()
    {
        FadeInOutManager.Instance.FadeIn(5f);
        
        yield return new WaitForSeconds(5.5f);
        SceneManager.LoadScene("OfficeJinwoo2");
    }

    public void ShowSpeechBubble(int npcNum)
    {
        for (int i = 0; i < npcTexts.Length; i++)
        {
            if (i == (npcNum - 1))
            {
                if (!npcTexts[i].gameObject.activeSelf)
                    npcTexts[i].gameObject.SetActive(true);
                npcTexts[i].StopAnim();
                npcTexts[i].EndCheck();

            }
            else
            {
                npcTexts[i].ClearText();
                npcTexts[i].gameObject.SetActive(false);
            }
        }
    }
}
