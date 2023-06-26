using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.Playables;
public class TalkBoyGuard : MonoSingleTon<TalkBoyGuard>
{
    private bool isMeetingStart = false;

    private float spacebarCoolTime = 1.5f;
    private float curCool = 0;

    [SerializeField] private int autoTalkingIndex = 1;

    [SerializeField] private TextAnim[] npcTexts;

    [SerializeField] private CinemachineVirtualCamera[] npcCam;

    [SerializeField] private CinemachineVirtualCamera meetingCam;

    private void Start()
    {
        AllClearText();

        //isMeetingStart = true;
        isMeetingStart = false;
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
    public void StartTalkBoyGuard()
    {
        foreach (var npc in npcTexts)
        {
            if (npc.gameObject.activeSelf && npc.isAnim)
            {
                npc.isSkip = true;
                return;
            }
        }
        CheckAutoTalkSpeechBubble();
        isMeetingStart = true;
    }
    public void CheckAutoTalkSpeechBubble()
    {
        switch (autoTalkingIndex)
        {
            case 1:
                ShowSpeechBubble(1);
                break;
            case 2:
                ShowSpeechBubble(3); //경호원
                break;
            case 3:
                ShowSpeechBubble(2);
                break;
            case 4:
                ShowSpeechBubble(3); //경호원
                break;
            case 5:
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
        FadeInOutManager.Instance.FadeIn(2.5f);
        yield return new WaitForSeconds(2.8f);
        SceneManager.LoadScene("MadScientistCutScene");
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

                npcCam[i].Priority += 2;
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
