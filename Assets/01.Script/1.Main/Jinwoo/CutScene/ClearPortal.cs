using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ClearPortal : MonoBehaviour
{
    public static bool isPortalCutscene = false;
    [SerializeField] private bool isCheckCollection = false;

    //[SerializeField] private GameObject playerSpawner;
    //[SerializeField] private GameObject keyDisplay;
    //[SerializeField] private Player player;
    //[SerializeField] private CinemachineVirtualCamera playerCam;
    [SerializeField] private List<GameObject> _enableList = new List<GameObject>();
    [SerializeField] private Transform playerSpawnpos;


    [SerializeField] private PlayableDirector completeCutscene;
    [SerializeField] private PlayableDirector notcompleteCutscene;

    public void Start()
    {
       // SaveDataManager.Instance.CurrentStageNameData.cutSceneDic[SaveDataManager.Instance.CurrentStageNameData.worldName];

    }
    public void StartCutScene()
    {
        if (MainMenuManager.isOpend) //메인메뉴 이미 열린겨
        {
            if (SaveDataManager.Instance.IsThisChapterClear(SaveDataManager.Instance.CurrentStageNameData.worldName))
            {
                //컷신실행
                if (!SaveDataManager.Instance.CheckPlayCutScene((SaveDataManager.Instance.CurrentStageNameData.worldName)))
                {
                    SaveDataManager.Instance.CurrentStageNameData.cutSceneDic[SaveDataManager.Instance.CurrentStageNameData.worldName] = true;
                    Debug.Log("컷씬 실행");
                    CheckCollectPiece();
                }
            }
           
            
        }

    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.U))
    //    {
    //        CheckCollectPiece();
    //    }
    //}
    public void CheckCollectPiece()
    {
        
        foreach (var item in _enableList)
        {
            item.SetActive(false);
        }


        //keyDisplay.SetActive(false);
        //player.gameObject.SetActive(false);
        //playerCam.gameObject.SetActive(false);

        //ISCLEARPORTAL이 트루면 조각 다 모은거, 폴스면 다 못모은거
        isCheckCollection = SaveDataManager.Instance.IsStageClearPortal(
            SaveDataManager.Instance.CurrentStageNameData.worldName,
            SaveDataManager.Instance.CurrentStageNameData.currentStageIndex);

        if (isCheckCollection == true) //좆ㅈ각 다 모음
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
        isPortalCutscene = false;

        foreach (var item in _enableList)
        {
            item.SetActive(true);
        }
        _enableList[2].gameObject.transform.position = playerSpawnpos.position;
        BGMManager.Instance.StopBGM();
        //keyDisplay.SetActive(true);
        //player.gameObject.SetActive(true);
        //playerCam.gameObject.SetActive(true);

    }
}
