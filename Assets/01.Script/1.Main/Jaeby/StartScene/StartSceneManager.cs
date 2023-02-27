using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _logoGroup = null;
    [SerializeField]
    private GameObject _mainPanel = null;

    private void Start()
    {
        LogoSplash();
    }

    private void LogoSplash()
    {
        _logoGroup.gameObject.SetActive(true);
        Sequence seq = DOTween.Sequence();
        seq.Append(_logoGroup.transform.DORotate(new Vector3(0, 0, 360f), 1f, RotateMode.FastBeyond360));
        seq.Join(_logoGroup.transform.DOShakeScale(0.8f));
        seq.Append(_logoGroup.DOFade(0f, 1f));
        seq.AppendCallback(() =>
        {
            _logoGroup.gameObject.SetActive(false);
            InitUI();
        });
    }

    private void InitUI()
    {
        _mainPanel.SetActive(true);
    }

    public void GoStageSelectScene()
    {
        LoadingSceneManager.LoadScene(1);
    }

    public void GoOption()
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
