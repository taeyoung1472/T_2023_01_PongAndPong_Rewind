using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class LastNPCTalkCutScene : MonoSingleTon<LastNPCTalkCutScene>
{
    private bool isTalkStart = false;

    private float spacebarCoolTime = 1.5f;
    private float curCool = 0;

    [SerializeField] private int autoTalkingIndex = 1;

    [SerializeField] private TextAnim[] npcTexts;


    [SerializeField] private CinemachineVirtualCamera meetingCam;

    private void Start()
    {
        AllClearText();

        //isMeetingStart = true;
        isTalkStart = false;
        autoTalkingIndex = 1;
    }
    private void Update()
    {
        if (isTalkStart)
        {
            curCool += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShowText();
                curCool = 0;
            }
        }
    }
    public void ShowText()
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
    }
    public void StartTalkNPC()
    {
        CheckAutoTalkSpeechBubble();
        isTalkStart = true;
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
                ShowSpeechBubble(2); 
                break;
            case 5:
                ShowSpeechBubble(2); 
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
                ShowSpeechBubble(2);
                break;
            case 10:
                ShowSpeechBubble(1); 
                break;
            case 11:
                ShowSpeechBubble(2); 
                break;
            case 12:
                ShowSpeechBubble(2); 
                break;
            case 13:
                ShowSpeechBubble(2); 
                break;
            case 14:
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

        isTalkStart = false;
        FadeInOutManager.Instance.FadeIn(2f);
        yield return new WaitForSeconds(2.1f);
        NextCutScene();

    }
    public void NextCutScene()
    {
        SceneManager.LoadScene("NewLab");
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
