using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;

public class MainMenuManager : MonoSingleTon<MainMenuManager>
{
    [Header("[RectTrans]")]
    [SerializeField] private RectTransform window;
    [SerializeField] private RectTransform content;
    GameObject curDisplayingWindow;

    [Header("[Vcam]")]
    [SerializeField] private CinemachineVirtualCamera menuCam;
    [SerializeField] private CinemachineVirtualCamera playerCam;

    [Header("[Player]")]
    [SerializeField] private GameObject player;

    public void Awake()
    {
        GameObject mainWindow;
        mainWindow = content.transform.Find("MainWindow").gameObject;
        mainWindow.SetActive(true);
        content.sizeDelta = new Vector2(content.sizeDelta.x, mainWindow.GetComponent<RectTransform>().sizeDelta.y);
        curDisplayingWindow = mainWindow;
    }

    public void Start()
    {
        player.SetActive(false);
    }

    public void ActiveWindow(GameObject targetWindow)
    {
        content.sizeDelta = new Vector2(content.sizeDelta.x, targetWindow.GetComponent<RectTransform>().sizeDelta.y);
        content.DOAnchorPos(Vector2.zero, 0.1f);
        targetWindow.SetActive(true);
        window.DOScale(Vector3.one, 0.2f);
        curDisplayingWindow = targetWindow;
    }

    public void CloseWindow()
    {
        window.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
        {
            content.DOAnchorPos(Vector2.zero, 0.1f);
            curDisplayingWindow?.SetActive(false);
        });
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void PlayGame()
    {
        menuCam.Priority = 0;
        playerCam.Priority = 1;
        player.SetActive(true);
    }
}
