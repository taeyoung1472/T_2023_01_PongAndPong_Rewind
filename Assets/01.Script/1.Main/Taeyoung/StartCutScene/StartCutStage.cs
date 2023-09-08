using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class StartCutStage : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private StartCutDialog[] dialogs;
    [HideInInspector] public bool isSkip;
    [HideInInspector] public bool isNext;
    [HideInInspector] public bool isActive;
    private int curDialogIndex;
    string curGeneratingText = "";
    string CurGeneratingText { get { return curGeneratingText; } set { curGeneratingText = value; StartCutStageController.Instance.text.SetText(curGeneratingText); } }

    public void Play()
    {
        vcam.gameObject.SetActive(true);
        curDialogIndex = 0;
        StartCoroutine(PlayDialog());
        isActive = true;
    }

    private void Update()
    {
        if (!isActive)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isNext)
            {
                isSkip = true;
            }
            isNext = true;
        }
    }

    IEnumerator PlayDialog()
    {
        while (curDialogIndex != dialogs.Length)
        {
            int curStringIndex = 0;
            while (CurGeneratingText != dialogs[curDialogIndex].text)
            {
                if (!isSkip)
                {
                    CurGeneratingText += dialogs[curDialogIndex].text[curStringIndex];
                    curStringIndex++;
                    yield return new WaitForSeconds(0.02f);
                }
                else
                {
                    CurGeneratingText = dialogs[curDialogIndex].text;
                }
            }

            yield return new WaitUntil(() => isNext);
            curDialogIndex++;
            CurGeneratingText = string.Empty;
            isNext = false;
            isSkip = false;
        }
        End();
    }

    public void End()
    {
        vcam.gameObject.SetActive(false);
        isActive = false;
    }
}

[Serializable]
public class StartCutDialog
{
    public string text;
    public UnityEvent dialogEvent;
}