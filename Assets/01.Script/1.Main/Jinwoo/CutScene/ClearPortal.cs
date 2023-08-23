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
        if (MainMenuManager.isOpend) //���θ޴� �̹� ������
        {
            //�ƽ� ��������
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

        //ISCLEARPORTAL�� Ʈ��� ���� �� ������ ������ �� ��������
        isCheckCollection = SaveDataManager.Instance.IsClearPortal(
             SaveDataManager.Instance.CurrentStageNameData.worldName, 
             SaveDataManager.Instance.CurrentStageNameData.stageCnt);
        
        if (isCheckCollection) //������ �� ����
        {
            completeCutscene.Play();
        }
        else //���� �� �� ����
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
