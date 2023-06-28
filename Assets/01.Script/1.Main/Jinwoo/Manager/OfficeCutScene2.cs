using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;

public class OfficeCutScene2 : MonoSingleTon<OfficeCutScene2>
{
    [SerializeField] private CinemachineVirtualCamera playerCam;
    [SerializeField] private Player player;
    [SerializeField] private GameObject playerCutScene;

    [SerializeField] private TextAnim playerText;
    [SerializeField] private TextAnim cellphoneText;

    [SerializeField] private Image exclamationMark;

    public GameObject checkCol;
    public bool isAnswerPhone = false;

    [SerializeField] private int autoTalkingIndex = 1;

    [SerializeField] private List<MonoBehaviour> enableList;

    [SerializeField] private float spacebarCoolTime = 1.5f;
    private float curCool = 0;

    [SerializeField] private string sceneName;

    void Start()
    {
        playerCutScene.SetActive(false);

        isAnswerPhone = false;
        autoTalkingIndex = 1;

        playerText.ClearText();
        playerText.gameObject.SetActive(false);
        cellphoneText.ClearText();
        cellphoneText.gameObject.SetActive(false);
        checkCol.SetActive(false);

        StartCoroutine(CellPhoneRing());
    }

    void Update()
    {
        if (isAnswerPhone)
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
        if (playerText.gameObject.activeSelf && playerText.isAnim)
        {
            playerText.isSkip = true;
        }
        else if (cellphoneText.gameObject.activeSelf && cellphoneText.isAnim)
        {
            cellphoneText.isSkip = true;
        }
        else
        {
            CheckAutoTalkSpeechBubble();

        }
    }
    public void DisableList()
    {
        foreach (var item in enableList)
        {
            item.enabled = false;
        }
    }
    public void AnswerCellPhone()
    {
        isAnswerPhone = true;
        UIGetter.Instance.PushUIs();


        exclamationMark.gameObject.SetActive(false);
        checkCol.gameObject.SetActive(false);
        playerCam.gameObject.SetActive(false);

        FadeInOutManager.Instance.FadeIn(1f);
        FadeInOutManager.Instance.FadeOut(1f);

        player.gameObject.SetActive(false);
        playerCutScene.SetActive(true);
        //player.transform.rotation = Quaternion.Euler(0, 90, 0);

        //player.Rigid.constraints = RigidbodyConstraints;

        //player.ForceStop();

        DisableList();

        ShowText();
    }
    IEnumerator CellPhoneRing()
    {
        yield return new WaitForSeconds(2f);
        exclamationMark.gameObject.SetActive(true);
        checkCol.SetActive(true);
    }
    public void CheckAutoTalkSpeechBubble()
    {
        switch (autoTalkingIndex)
        {
            case 1:
                ShowSpeechBubble(true);
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
                ShowSpeechBubble(false);
                break;
            case 6:
                ShowSpeechBubble(true);
                break;
            case 7:
                ShowSpeechBubble(false);
                break;
            case 8:
                EndTalk();
                break;
            default:
                break;
        }
        autoTalkingIndex++;
    }
    private void EndTalk()
    {
        playerText.ClearText();
        playerText.gameObject.SetActive(false);

        cellphoneText.ClearText();
        cellphoneText.gameObject.SetActive(false);

        isAnswerPhone = false;
        FadeInOutManager.Instance.FadeIn(2f);

        StartCoroutine(NextCutScene());
    }
    public IEnumerator NextCutScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneName);
    }
    public void ShowSpeechBubble(bool isPlayer)
    {
        if (isPlayer)
        {
            cellphoneText.ClearText();
            cellphoneText.gameObject.SetActive(false);
            playerText.gameObject.SetActive(true);

            playerText.StopAnim();
            playerText.EndCheck();
        }
        else
        {
            playerText.ClearText();
            playerText.gameObject.SetActive(false);
            cellphoneText.gameObject.SetActive(true);

            cellphoneText.StopAnim();
            cellphoneText.EndCheck();
        }
    }
}
