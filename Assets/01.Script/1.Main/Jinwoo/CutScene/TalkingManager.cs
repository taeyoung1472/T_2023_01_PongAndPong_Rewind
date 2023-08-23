using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class TalkingManager : MonoBehaviour
{
    private bool isTalkStart = false;

    private float curCool = 0;

    [SerializeField] private int talkNum = 0;

    [SerializeField] private int autoTalkingIndex = 1;
    [SerializeField] private int autoTalkingTotalCnt = 0;

    [SerializeField] private List<TextNPCArrary> list = new List<TextNPCArrary>();
    [SerializeField] private TextAnimCutScene[] npcTexts;


    public UnityEvent focusCollection = new UnityEvent();
    public void StartAutoTalking()
    {
        StartCoroutine(StartTalking());
    }
    private IEnumerator StartTalking()
    {
        AllClearText();

        npcTexts = list[talkNum++].npcTexts;

        autoTalkingIndex = 1;
        autoTalkingTotalCnt = 0;
        foreach (var npc in npcTexts)
        {

            autoTalkingTotalCnt += npc.TextData.cutSceneTexts.Length;
        }

        yield return new WaitForSeconds(.5f);
        isTalkStart = true;
        CheckAutoTalkSpeechBubble();
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
    public void CheckAutoTalkSpeechBubble()
    {
        if (autoTalkingIndex == autoTalkingTotalCnt + 1)
        {
            StartCoroutine(EndTalk());
            return;
        }
        for (int i = 0; i < npcTexts.Length; i++) //대화 캐릭 몇명인지
        {
            for (int j = 0; j < npcTexts[i].TextData.cutSceneTexts.Length; j++) // 총 대화수에서 맞는 대화 출력
            {
                if (npcTexts[i].TextData.cutSceneTexts[j].textOrder == autoTalkingIndex)
                {
                    ShowSpeechBubble(i);
                    return;
                }
            }

        }

    }

    private IEnumerator EndTalk()
    {
        yield return new WaitForSeconds(1f);
        if (talkNum == 1) //조각 다 못모음
        {
            isTalkStart = false;
            //focusPiece.Play();
            focusCollection?.Invoke();
        }
        
    }

    private void AllClearText()
    {
        foreach (var text in npcTexts)
        {
            text.ClearText();
            text.gameObject.SetActive(false);
        }
    }

    public void ShowSpeechBubble(int npcNum)
    {
        for (int i = 0; i < npcTexts.Length; i++)
        {
            if (i == npcNum)
            {
                if (!npcTexts[i].gameObject.activeSelf)
                    npcTexts[i].gameObject.SetActive(true);
                npcTexts[i].StopAnim();
                npcTexts[i].EndCheck(autoTalkingIndex);
                autoTalkingIndex++;

                if (npcTexts[i].npcCam != null)
                    npcTexts[i].npcCam.Priority += 2;
            }
            else
            {
                npcTexts[i].ClearText();
                npcTexts[i].gameObject.SetActive(false);

                if (npcTexts[i].npcCam != null)
                    npcTexts[i].npcCam.Priority = 10;
            }
        }
    }
}
