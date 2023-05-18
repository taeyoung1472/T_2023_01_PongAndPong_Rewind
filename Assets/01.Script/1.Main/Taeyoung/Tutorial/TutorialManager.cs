using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoSingleTon<TutorialManager>
{
    private static TutorialState curTutorialState;

    [SerializeField] private GameObject gameCam;

    [Space(15)]

    [Header("제목")]
    [SerializeField] private Transform titlePanel;
    [SerializeField] private TextMeshPro titleText;
    [SerializeField] private TextMeshPro subTitleText;

    [Header("요약")]
    [SerializeField] private Transform startPanel;
    [SerializeField] private TextMeshPro startText;

    [Header("마무리")]
    [SerializeField] private Transform endPanel;
    [SerializeField] private TextMeshPro endText;

    [Header("끝")]
    [SerializeField] private Transform endTitlePanel;
    [SerializeField] private TextMeshPro endTitleText;

    [SerializeField] private TutorialInfo curTutoInfo;

    public void Start()
    {
        ChangeState(curTutorialState);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ChangeState(TutorialState.Title);
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

        titlePanel.gameObject.SetActive(false);
        ChangeState(TutorialState.Start);
    }

    public IEnumerator OnStart()
    {
        startPanel.gameObject.SetActive(true);

        startText.SetText(curTutoInfo.tutoStartText);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return null;

        startPanel.gameObject.SetActive(false);
        ChangeState(TutorialState.Game);
    }

    public IEnumerator OnGame()
    {
        gameCam.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        StageManager.stageDataSO = curTutoInfo.stageData;
        LoadingSceneManager.LoadScene(10);
        curTutorialState = TutorialState.End;
    }

    public IEnumerator OnEnd()
    {
        endPanel.gameObject.SetActive(true);

        endText.SetText(curTutoInfo.tutoEndText);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return null;

        endPanel.gameObject.SetActive(false);
        ChangeState(TutorialState.EndTitle);
    }

    public IEnumerator OnEndTitle()
    {
        endTitlePanel.gameObject.SetActive(true);

        endTitleText.SetText($"[{curTutoInfo.tutoTitle}] 학습 완료");

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return null;

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