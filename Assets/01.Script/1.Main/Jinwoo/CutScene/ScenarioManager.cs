using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
[Serializable]
public class TextNPCArrary
{
    public TextAnimCutScene[] npcTexts;
}

public class ScenarioManager : MonoSingleTon<ScenarioManager>
{
    #region ������
    private bool isTalkStart = false;

    private float curCool = 0;

    [SerializeField] private int talkNum = 0; 

    [SerializeField] private int autoTalkingIndex = 1;
    [SerializeField] private int autoTalkingTotalCnt = 0;

    [SerializeField] private List<TextNPCArrary> list = new List<TextNPCArrary>();
    [SerializeField] private TextAnimCutScene[] npcTexts;

    [SerializeField] private TextAnim beforeDay;

    [SerializeField] private PlayableDirector labNpcTalk;
    [SerializeField] private PlayableDirector officeCutScene1;
    [SerializeField] private PlayableDirector meetingCutScene;
    [SerializeField] private PlayableDirector portalCutScene;
    [SerializeField] private PlayableDirector enterMadCutScene;
    [SerializeField] private PlayableDirector madCutScene;


    [SerializeField] private GameObject meetingCam;
    [SerializeField] private GameObject meetingNPC;

    #endregion
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
        foreach (var npc  in npcTexts)
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
        if (autoTalkingIndex == autoTalkingTotalCnt+1)
        {
            StartCoroutine(EndTalk());
            return;
        }
        for (int i = 0; i < npcTexts.Length; i++) //��ȭ ĳ�� �������
        {
            for (int j = 0; j < npcTexts[i].TextData.cutSceneTexts.Length; j++) // �� ��ȭ������ �´� ��ȭ ���
            {
                if (npcTexts[i].TextData.cutSceneTexts[j].textOrder == autoTalkingIndex)
                {
                    ShowSpeechBubble(i);
                    return;
                }
            }
            
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
    private IEnumerator EndTalk()
    {
        AllClearText();

        isTalkStart = false;

        switch (talkNum)
        {
            case 1:// ù���� ������
                JinwooVolumeManager.Instance.StartFadeInCinematicBars();
                yield return new WaitForSeconds(2f);
                beforeDay.gameObject.SetActive(true);
                beforeDay.EndCheck();
                yield return new WaitForSeconds(3.5f);
                beforeDay.gameObject.SetActive(false);
                yield return new WaitForSeconds(.5f);
                JinwooVolumeManager.Instance.StartFadeOutCinematicBars(true);
                labNpcTalk.Play();
                break;
            case 2: //��Żȸ�� ������
                FadeInOutManager.Instance.FadeIn(1.5f);
                yield return new WaitForSeconds(1f);
                JinwooVolumeManager.Instance.DirectDisableCinematicBars();
                yield return new WaitForSeconds(1f);
                officeCutScene1.Play();
                yield return new WaitForSeconds(1f);
                FadeInOutManager.Instance.FadeOut(2f);
                break;
            case 3://ù��° �繫�� �޴��� ��ȭ ������
                FadeInOutManager.Instance.FadeIn(1.5f);
                yield return new WaitForSeconds(2f);
                FadeInOutManager.Instance.FadeOut(2f);
                meetingCam.SetActive(true);
                meetingNPC.SetActive(true);

                OfficeManager.Instance.EndPhone();
                yield return new WaitForSeconds(1f);
                StartAutoTalking();
                break;
            case 4://ȸ�ǽ� ��ȭ ������
                FadeInOutManager.Instance.FadeIn(1f);
                yield return new WaitForSeconds(1.5f);
                meetingCutScene.Play();
                break;
            case 5:// ���ȿ�� ��ȭ ������
                FadeInOutManager.Instance.FadeIn(1.5f);
                yield return new WaitForSeconds(2f);
                meetingCam.SetActive(false);
                meetingNPC.SetActive(false);
                FadeInOutManager.Instance.FadeOut(1f);
                portalCutScene.Play();
                break;
            case 6://��Ż�濡�� �� npc ��ȭ ������
                yield return new WaitForSeconds(1f);
                enterMadCutScene.Play();
                break;
            case 7://�ŵ� ���̾�Ƽ��Ʈ ȥ�㸻 ������
                yield return new WaitForSeconds(1f);
                madCutScene.Play();
                break;
            case 8: //cctv ��ȭ �� ������
                FadeInOutManager.Instance.FadeIn(1.5f);
                yield return new WaitForSeconds(2f);
                FadeInOutManager.Instance.FadeOut(1.5f);
                officeCutScene1.Play();
                break;
            case 9:

                break;
            default:
                break;
        }

        /*if (talkNum == 1)// ù���� ������
        {
            JinwooVolumeManager.Instance.StartFadeInCinematicBars();
            yield return new WaitForSeconds(2f);
            beforeDay.gameObject.SetActive(true);
            beforeDay.EndCheck();
            yield return new WaitForSeconds(3.5f);
            beforeDay.gameObject.SetActive(false);
            yield return new WaitForSeconds(.5f);
            JinwooVolumeManager.Instance.StartFadeOutCinematicBars(true);
            labNpcTalk.Play();
        }
        else if (talkNum == 2) //��Żȸ�� ������
        {
            FadeInOutManager.Instance.FadeIn(1.5f);
            yield return new WaitForSeconds(1f);
            JinwooVolumeManager.Instance.DirectDisableCinematicBars();
            yield return new WaitForSeconds(1f);
            officeCutScene1.Play();
            yield return new WaitForSeconds(1f);
            FadeInOutManager.Instance.FadeOut(2f);
        }
        else if(talkNum == 3) //ù��° �繫�� �޴��� ��ȭ ������
        {
            FadeInOutManager.Instance.FadeIn(1.5f);
            yield return new WaitForSeconds(2f);
            FadeInOutManager.Instance.FadeOut(2f);
            meetingCam.SetActive(true);
            meetingNPC.SetActive(true);

            OfficeManager.Instance.EndPhone();
            yield return new WaitForSeconds(1f);
            StartAutoTalking();
        }
        else if(talkNum == 4) //ȸ�ǽ� ��ȭ ������
        {
            FadeInOutManager.Instance.FadeIn(1f);
            yield return new WaitForSeconds(1.5f);
            meetingCutScene.Play();
        }
        else if(talkNum == 5) // ���ȿ�� ��ȭ ������
        {
            FadeInOutManager.Instance.FadeIn(1.5f);
            yield return new WaitForSeconds(2f);
            meetingCam.SetActive(false);
            meetingNPC.SetActive(false);
            FadeInOutManager.Instance.FadeOut(1f);
            portalCutScene.Play();
        }
        else if (talkNum == 6) //��Ż�濡�� �� npc ��ȭ ������
        {
            JinwooVolumeManager.Instance.StartFadeInCinematicBars();
            yield return new WaitForSeconds(2f);
            JinwooVolumeManager.Instance.StartFadeOutCinematicBars(false);
        }
        else if(talkNum == 7)
        {

        }*/



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

                if(npcTexts[i].npcCam != null)
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
