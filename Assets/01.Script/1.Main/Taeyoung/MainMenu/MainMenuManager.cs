using Cinemachine;
using DG.Tweening;
using Highlighters;
using System.Collections;
using UnityEngine;

public class MainMenuManager : MonoSingleTon<MainMenuManager>
{
    public static bool isOpend;
    bool isActive = true;
    bool isWindowActive = false;

    [Header("[RectTrans]")]
    [SerializeField] private RectTransform window;
    [SerializeField] private RectTransform content;
    [SerializeField] private Transform screen;
    [SerializeField] private Transform tabletPosition;
    [SerializeField] private Transform labtopPosition;
    GameObject curDisplayingWindow;

    [Header("[Vcam]")]
    [SerializeField] private CinemachineVirtualCamera menuCam;
    [SerializeField] private CinemachineVirtualCamera playerCam;
    [SerializeField] private CinemachineVirtualCamera tabletCam;

    [Header("[Player]")]
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator tabletAnimator;

    public IEnumerator Start()
    {
        if (isOpend)
        {
            PlayGame();
            yield break;
        }
        isOpend = true;

        yield return null;

        player.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isActive)
            {
                if (isWindowActive)
                {
                    WindowClose();
                }
                else
                {
                    PlayGame();
                }
            }
            else if(Define.player.PlayerActionCheck(PlayerActionType.Interact) == false)
            {
                OpenMenu();
            }
        }
        if (isActive)
        {
            screen.transform.position = tabletPosition.position;
            screen.transform.rotation = tabletPosition.rotation;
            screen.transform.localScale = tabletPosition.localScale;
        }
        else
        {
            screen.transform.position = labtopPosition.position;
            screen.transform.rotation = labtopPosition.rotation;
            screen.transform.localScale = labtopPosition.localScale;
        }
    }

    public void WindowActive(GameObject targetWindow)
    {
        curDisplayingWindow?.SetActive(false);
        content.sizeDelta = new Vector2(content.sizeDelta.x, targetWindow.GetComponent<RectTransform>().sizeDelta.y);
        content.DOAnchorPos(Vector2.zero, 0.1f);
        targetWindow.SetActive(true);
        window.DOScale(Vector3.one, 0.2f);
        curDisplayingWindow = targetWindow;
        isWindowActive = true;
    }

    public void WindowChange(GameObject targetWindow)
    {
        curDisplayingWindow?.SetActive(false);
        content.sizeDelta = new Vector2(content.sizeDelta.x, targetWindow.GetComponent<RectTransform>().sizeDelta.y);
        content.DOAnchorPos(Vector2.zero, 0.1f);
        targetWindow.SetActive(true);
        curDisplayingWindow = targetWindow;
    }

    public void WindowClose()
    {
        window.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
        {
            content.DOAnchorPos(Vector2.zero, 0.1f);
            curDisplayingWindow?.SetActive(false);
        });
        isWindowActive = false;
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
        player.GetComponent<Highlighter>().enabled = true;
        playerInput.enabled = true;

        isActive = false;
        playerAnimator.SetLayerWeight(2, 0);
        playerAnimator.GetComponent<AnimationIK>().SetIKWeightZero();
        playerAnimator.SetBool("IsHolding", false);
        WindowClose();

        tabletAnimator.SetBool("IsOpen", false);
    }

    public void OpenMenu()
    {
        tabletCam.Priority = 1;
        playerCam.Priority = 0;

        player.GetComponent<Player>().ForceStop();
        player.GetComponent<Highlighter>().enabled = false;
        playerInput.enabled = false;

        isActive = true;
        playerAnimator.SetLayerWeight(2, 1);
        playerAnimator.GetComponent<AnimationIK>().SetIKWeightOne();
        playerAnimator.SetBool("IsHolding", true);

        this.Invoke(() => tabletAnimator.SetBool("IsOpen", true), 0.5f);
    }
}
