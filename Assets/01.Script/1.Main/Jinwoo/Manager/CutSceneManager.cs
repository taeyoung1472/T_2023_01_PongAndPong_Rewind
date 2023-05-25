using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
//using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneManager : MonoSingleTon<CutSceneManager>
{
    [SerializeField] private TalkingCheck[] npc;
    public PlayableDirector playerCutScene;

    [SerializeField] private List<MonoBehaviour> enableList;

    [SerializeField] private Player player;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private RuntimeAnimatorController talkAnimator;

    private bool isAutoTalking = false;
    [SerializeField] private int autoTalkingIndex = 0;


    [SerializeField] private TextAnim playerTalktext;
    [SerializeField] private TextAnim npcTalktext;

    private float spacebarCoolTime = 1f;
    private float curCool = 0;

    private int skipToggle;

    [SerializeField] private PlayableDirector elevatorCutScene;
    [SerializeField] private PlayableDirector npcTalkCutScene;

    [SerializeField] private Collider[] cutsceneCheck;
    void Awake()
    {
        //MainMenuManager.isOpend = true;
        FadeInOutManager.Instance.FadeOut(2f);
        curCool = 0;
        isAutoTalking = false;
        autoTalkingIndex = 0;
        playerTalktext.gameObject.SetActive(false);
        npcTalktext.gameObject.SetActive(false);

        DisableCollider();
    }

    void Update()
    {
        
        if (isAutoTalking)
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
        if (playerTalktext.gameObject.activeSelf && playerTalktext.isAnim)
        {
            playerTalktext.isSkip = true;
        }
        else if (npcTalktext.gameObject.activeSelf && npcTalktext.isAnim)
        {
            npcTalktext.isSkip = true;
        }
        else
        {
                CheckAutoTalkSpeechBubble();

        }
    }
    public void CheckAllTalkNPC()
    {
        int successNpc = 0;
        for (int i = 0; i < npc.Length; i++)
        {
            if (npc[i].GetIsTalk())
            {
                successNpc++;
            }
        }

        if (successNpc >= npc.Length) //¸ðµç NPC¿Í ´ëÈ­ÇÔ
        {
            Debug.Log("ÄÆ¾À½ÃÀÛ!!" + successNpc);

            playerCutScene.Play();

            cutsceneCheck[0].gameObject.SetActive(true);
            cutsceneCheck[1].gameObject.SetActive(true);

            UIGetter.Instance.PushUIs();
        }
        else 
        {
           
        }
    }

    public void StartElevatorCutScene()
    {
        DisableCollider();
        elevatorCutScene.Play();
    }
    public void DisableCollider()
    {
        cutsceneCheck[0].gameObject.SetActive(false);
        cutsceneCheck[1].gameObject.SetActive(false);
    }
    public void PlayerEnableList()
    {
        foreach (var item in enableList)
        {
            item.enabled = true;
        }
        
    }
    public void PlayerDisableList()
    {
        foreach (var item in enableList)
        {
            item.enabled = false;
        }
        
    }
    public void PlayerTurn()
    {
        player.transform.rotation = Quaternion.Euler(0, 90, 0);
    }
    IEnumerator PlayerAloneCutScene()
    {
        yield return new WaitForSeconds(2f);
        playerCutScene.Play();
    }

    public void CheckAutoTalkSpeechBubble()
    {
        switch (autoTalkingIndex)
        {
            case 0:
                ShowSpeechBubble(true);
                break;
            case 1:
                ShowSpeechBubble(false);
                break;
            case 2:
                ShowSpeechBubble(false);
                break;
            case 3:
                ShowSpeechBubble(true);
                break;
            case 4:
                ShowSpeechBubble(false);
                break;
            case 5:
                ShowSpeechBubble(true);
                break;
            case 6:
                ShowSpeechBubble(false);
                break;
            case 7:
                ShowSpeechBubble(true);
                break;
            case 8:
                ShowSpeechBubble(false);
                break;
            case 9:
                ShowSpeechBubble(false);
                break;
            case 10:
                ShowSpeechBubble(false);
                break;
            case 11:
                ShowSpeechBubble(true);
                break;
            case 12:
                ShowSpeechBubble(false);
                break;
            case 13:
                ShowSpeechBubble(false);
                break;
            case 14:
                ShowSpeechBubble(true);
                break;
            case 15:
                ShowSpeechBubble(false);
                break;
            case 16:
                ShowSpeechBubble(false);
                break;
            case 17:
                ShowSpeechBubble(true);
                break;
            case 18:
                ShowSpeechBubble(false);
                break;
            case 19:
                ShowSpeechBubble(true);
                break;
            case 20:
                ShowSpeechBubble(false);
                break;
            case 21:
                ShowSpeechBubble(true);
                break;
            case 22:
                ShowSpeechBubble(false);
                break;
            case 23:
                ShowSpeechBubble(true);
                break;
            case 24:
                EndAutoTalk();
                break;

        }
        autoTalkingIndex++;
    }
    public void EndAutoTalk()
    {
        playerTalktext.ClearText();
        npcTalktext.ClearText();

        playerTalktext.gameObject.SetActive(false);
        npcTalktext.gameObject.SetActive(false);

        isAutoTalking = false;

        npcTalkCutScene.Play();
    }
    public void ShowSpeechBubble(bool isPlayer)
    {
        if (isPlayer)
        {
            npcTalktext.ClearText();
            npcTalktext.gameObject.SetActive(false);


            playerTalktext.gameObject.SetActive(true);
            playerTalktext.StopAnim();
            playerTalktext.EndCheck();
        }
        else
        {
            playerTalktext.ClearText();
            playerTalktext.gameObject.SetActive(false);

            npcTalktext.gameObject.SetActive(true);
            npcTalktext.StopAnim();
            npcTalktext.EndCheck();
        }
    }

    public void StartNPCAndPlayerAutoTalk()
    {
        player.Rigid.constraints = RigidbodyConstraints.FreezeAll;

        playerAnimator.runtimeAnimatorController = talkAnimator;
        playerTalktext.gameObject.SetActive(true);
        npcTalktext.gameObject.SetActive(true);

        playerTalktext.ClearText();
        npcTalktext.ClearText();

        isAutoTalking = true;
        CheckAutoTalkSpeechBubble();
    }

    public void NextCutScene()
    {
        StartCoroutine(NextScene());
    }
    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Lab NPCMeeting");
    }
}
