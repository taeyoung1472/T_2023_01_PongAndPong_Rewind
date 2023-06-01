using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoSingleTon<TutorialManager>
{
    private static TutorialInfo curTutoInfo;
    public static TutorialInfo CurTutoInfo
    {
        get { return curTutoInfo; }
        set
        {
            curTutoInfo = value;
            if (curTutorialState == TutorialState.None)
            {
                curTutorialState = TutorialState.Title;
            }
        }
    }
    private static TutorialState curTutorialState;

    [SerializeField] private GameObject gameCam;
    [SerializeField] private GameObject tutoCam;
    [SerializeField] private PlayerInput playerInput;

    [Space(15)]

    [Header("제목")]
    [SerializeField] private Transform titlePanel;
    [SerializeField] private TextMeshPro titleText;
    [SerializeField] private TextMeshPro subTitleText;

    [Header("끝")]
    [SerializeField] private Transform endTitlePanel;
    [SerializeField] private TextMeshPro endTitleText;

    [Header("끝")]
    [SerializeField] private TextAnim_Tuto textAnim;

    public void Start()
    {
        if (curTutoInfo != null)
        {
            ChangeState(curTutorialState);
            if (curTutorialState != TutorialState.None)
            {
                tutoCam.SetActive(true);
            }
        }
    }

    private void Update()
    {
        if (curTutorialState != TutorialState.None)
        {
            playerInput.enabled = false;
        }
        else
        {
            playerInput.enabled = true;
        }
    }

    #region FSM
    public void ChangeState(TutorialState state)
    {
        curTutorialState = state;
        switch (curTutorialState)
        {
            case TutorialState.Title:
                StartCoroutine(OnTitle());
                break;
            case TutorialState.Start:
                StartCoroutine(OnStart());
                break;
            case TutorialState.Game:
                StartCoroutine(OnGame());
                break;
            case TutorialState.End:
                StartCoroutine(OnEnd());
                break;
            case TutorialState.EndTitle:
                StartCoroutine(OnEndTitle());
                break;
        }
    }

    public IEnumerator OnTitle()
    {
        titlePanel.gameObject.SetActive(true);

        titleText.SetText(curTutoInfo.tutoTitle);
        subTitleText.SetText(curTutoInfo.tutoSubTitle);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return null;

        ChangeState(TutorialState.Start);
    }

    public IEnumerator OnStart()
    {
        int textLength = curTutoInfo.startNpcText.Length;

        for (int i = 0; i < textLength; i++)
        {
            textAnim.SetText(curTutoInfo.startNpcText[i]);

            yield return new WaitUntil(() => textAnim.IsEnd());

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        yield return null;

        titlePanel.gameObject.SetActive(false);
        ChangeState(TutorialState.Game);
    }

    public IEnumerator OnGame()
    {
        gameCam.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        StageManager.stageDataSO = curTutoInfo.stageData;
        curTutorialState = TutorialState.End;
        LoadingSceneManager.LoadScene(10);
    }

    public IEnumerator OnEnd()
    {
        endTitlePanel.gameObject.SetActive(true);
        endTitleText.SetText($"[{curTutoInfo.tutoTitle}] 학습 완료");

        int textLength = curTutoInfo.endNpcText.Length;

        for (int i = 0; i < textLength; i++)
        {
            textAnim.SetText(curTutoInfo.endNpcText[i]);

            yield return new WaitUntil(() => textAnim.IsEnd());

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return null;

        ChangeState(TutorialState.EndTitle);
    }

    public IEnumerator OnEndTitle()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return null;

        curTutoInfo = null;
        curTutorialState = TutorialState.None;
        tutoCam.SetActive(false);
        playerInput.enabled = true;

        endTitlePanel.gameObject.SetActive(false);
    }

    #endregion
}
public enum TutorialState
{
    None,
    Title,
    Start,
    Game,
    End,
    EndTitle
}