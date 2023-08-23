using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ClearPortal : MonoBehaviour
{
    public static bool isPortalCutscene = false;
    [SerializeField] private bool isCheckCollection = false;

    [SerializeField] private GameObject keyDisplay;
    [SerializeField] private Player player;
    [SerializeField] private Transform playerSpawnpos;
    [SerializeField] private CinemachineVirtualCamera playerCam;

    [SerializeField] private PlayableDirector completeCutscene;
    [SerializeField] private PlayableDirector notcompleteCutscene;

    private void Start()
    {
        //AllCollectPiece();
        //isAllpiece = false;
        if (MainMenuManager.isOpend) //메인메뉴 이미 열린겨
        {
            //컷신 실행인지
            if (isPortalCutscene)
            {
                CheckCollectPiece();
            }
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            CheckCollectPiece();
        }
    }
    public void CheckCollectPiece()
    {
        isPortalCutscene = false;

        keyDisplay.SetActive(false);
        player.gameObject.SetActive(false);
        playerCam.gameObject.SetActive(false);

        //ISCLEARPORTAL이 트루면 조각 다 모은거 폴스면 다 못모은거
        isCheckCollection = SaveDataManager.Instance.IsClearPortal(
             SaveDataManager.Instance.CurrentStageNameData.worldName, 
             SaveDataManager.Instance.CurrentStageNameData.stageCnt);
        
        if (isCheckCollection) //좆ㅈ각 다 모음
        {
            completeCutscene.Play();
        }
        else //좆각 다 못 모음
        {
            notcompleteCutscene.Play();
        }

    }

    public void FocusCollection()
    {
        keyDisplay.SetActive(true);
        player.gameObject.SetActive(true);
        player.gameObject.transform.position = playerSpawnpos.position;
        playerCam.gameObject.SetActive(true);

    }
}
