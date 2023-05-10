using System.Collections;
//using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCMeetingCutSceneManager : MonoSingleTon<NPCMeetingCutSceneManager>
{
    private Player player;

    private bool isMeetingStart = false;

    private float spacebarCoolTime = 1.5f;
    private float curCool = 0;

    [SerializeField] private int autoTalkingIndex = 1;

    [SerializeField] private TextAnim[] npcs;

    private IEnumerator Start()
    {

        player = FindObjectOfType<Player>();
        player.gameObject.SetActive(false);

        AllClear();

        isMeetingStart = true;
        autoTalkingIndex = 1;

        yield return new WaitForSeconds(1.5f);
        CheckAutoTalkSpeechBubble();
    }

    public void AllClear()
    {
        foreach (var npc in npcs)
        {
            npc.ClearText();
            npc.gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if (isMeetingStart)
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
        foreach (var npc in npcs)
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
                ShowSpeechBubble(3);
                break;
            case 6:
                ShowSpeechBubble(4);
                break;
            case 7:
                ShowSpeechBubble(1);
                break;
            case 8:
                ShowSpeechBubble(2);
                break;
            case 9:
                ShowSpeechBubble(1);
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
                ShowSpeechBubble(3);
                break;
            case 14:
                ShowSpeechBubble(4);
                break;
            case 15:
                ShowSpeechBubble(1);
                break;
            case 16:
                ShowSpeechBubble(2);
                break;
            case 17:
                ShowSpeechBubble(2);
                break;
            case 18:
                ShowSpeechBubble(1);
                break;
            case 19:
                ShowSpeechBubble(1);
                break;
            case 20:
                ShowSpeechBubble(1);
                break;
            case 21:
                ShowSpeechBubble(1);
                break;
            case 22:
                ShowSpeechBubble(2);
                break;
            case 23:
                ShowSpeechBubble(3);
                break;
            case 24:
                ShowSpeechBubble(2);
                break;
            case 25:
                ShowSpeechBubble(4);
                break;
            case 26:
                ShowSpeechBubble(1);
                break;
            case 27:
                ShowSpeechBubble(2);
                break;
            case 28:
                ShowSpeechBubble(4);
                break;
            case 29:
                ShowSpeechBubble(3);
                break;
            case 30:
                ShowSpeechBubble(1);
                break;
            case 31:
                ShowSpeechBubble(2);
                break;
            case 32:
                ShowSpeechBubble(1);
                break;
            case 33:
                ShowSpeechBubble(4);
                break;
            case 34:
                EndTalk();
                break;

        }
        autoTalkingIndex++;
    }

    private void EndTalk()
    {
        AllClear();

        isMeetingStart = false;
        FadeInOutManager.Instance.FadeIn(5f);

        StartCoroutine(NextCutScene());
    }
    public IEnumerator NextCutScene()
    {
        yield return new WaitForSeconds(1f);
        JinwooVolumeManager.Instance.EndCinmaticBars();
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("OfficeJinwoo");
    }

    public void ShowSpeechBubble(int npcNum)
    {

        for (int i = 0; i < npcs.Length; i++)
        {
            if (i == (npcNum - 1))
            {
                if (!npcs[i].gameObject.activeSelf)
                    npcs[i].gameObject.SetActive(true);
                npcs[i].StopAnim();
                npcs[i].EndCheck();
            }
            else
            {
                npcs[i].ClearText();
                npcs[i].gameObject.SetActive(false);
            }
        }
        //switch (npcNum)
        //{
        //    case 1:
        //        npc1.StopAnim();
        //        npc1.EndCheck();

        //        break;
        //    case 2:
        //        if(!npc2.gameObject.activeSelf)
        //            npc2.gameObject.SetActive(true);
        //        npc2.StopAnim();
        //        npc2.EndCheck();

        //        break;
        //    case 3:
        //        if (!npc3.gameObject.activeSelf)
        //            npc3.gameObject.SetActive(true);
        //        npc3.StopAnim();
        //        npc3.EndCheck();

        //        break;
        //    case 4:
        //        if (!npc4.gameObject.activeSelf)
        //            npc4.gameObject.SetActive(true);
        //        npc4.StopAnim();
        //        npc4.EndCheck();

        //        break;
        //    default:
        //        break;
        //}
    }
}
