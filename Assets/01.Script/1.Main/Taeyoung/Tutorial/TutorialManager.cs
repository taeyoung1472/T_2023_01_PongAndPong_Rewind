using System;
using System.Collections;
using UnityEngine;

public class TutorialManager : MonoSingleTon<TutorialManager>
{
    private TutorialState curTutorialState;

    [SerializeField] private Transform titlePanel;
    [SerializeField] private Transform startPanel;
    [SerializeField] private Transform videoPanel;
    [SerializeField] private Transform endPanel;
    [SerializeField] private Transform choicePanel;
    [SerializeField] private Transform endTitlePanel;

    private bool isChoice;
    private bool isReplayVideo;

    public void Start()
    {
        ChangeState(TutorialState.Title);
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
            case TutorialState.Video:
                StartCoroutine(OnVideo());
                break;
            case TutorialState.End:
                StartCoroutine(OnEnd());
                break;
            case TutorialState.Choice:
                StartCoroutine(OnChoice());
                break;
            case TutorialState.EndTitle:
                StartCoroutine(OnEndTitle());
                break;
        }
    }

    public IEnumerator OnTitle()
    {
        titlePanel.gameObject.SetActive(true);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return null;

        titlePanel.gameObject.SetActive(false);
        ChangeState(TutorialState.Start);
    }

    public IEnumerator OnStart()
    {
        startPanel.gameObject.SetActive(true);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return null;

        startPanel.gameObject.SetActive(false);
        ChangeState(TutorialState.Video);
    }

    public IEnumerator OnVideo()
    {
        videoPanel.gameObject.SetActive(true);

        // 영상 처음부터 끝 까지
        // 프레임 단위로 조작 가능한 상태

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return null;

        videoPanel.gameObject.SetActive(false);
        ChangeState(TutorialState.End);
    }

    public IEnumerator OnEnd()
    {
        endPanel.gameObject.SetActive(true);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return null;

        endPanel.gameObject.SetActive(false);
        ChangeState(TutorialState.Choice);
    }

    public IEnumerator OnChoice()
    {
        choicePanel.gameObject.SetActive(true);
        isChoice = false;

        yield return new WaitUntil(() => isChoice);
        yield return null;

        choicePanel.gameObject.SetActive(false);
        if (isReplayVideo)
        {
            ChangeState(TutorialState.Video);
        }
        else
        {
            ChangeState(TutorialState.EndTitle);
        }

        isChoice = false;
    }

    public IEnumerator OnEndTitle()
    {
        endTitlePanel.gameObject.SetActive(true);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return null;

        endTitlePanel.gameObject.SetActive(false);
    }

    #endregion

    [ContextMenu("Choice")]
    public void A()
    {
        Choice(false);
    }

    public void Choice(bool value)
    {
        isChoice = true;
        isReplayVideo = value;
    }
}
public enum TutorialState
{
    Title,
    Start,
    Video,
    End,
    Choice,
    EndTitle
}