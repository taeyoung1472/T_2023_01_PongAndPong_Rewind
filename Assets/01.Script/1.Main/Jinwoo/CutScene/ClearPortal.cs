using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ClearPortal : MonoBehaviour
{
    public static bool isAllpiece = false;

    [SerializeField] private Player player;
    [SerializeField] private Transform playerSpawnpos;
    [SerializeField] private CinemachineVirtualCamera playerCam;

    [SerializeField] private PlayableDirector completeCutscene;
    [SerializeField] private PlayableDirector notcompleteCutscene;

    private void Start()
    {
        //AllCollectPiece();
        //isAllpiece = false;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            AllCollectPiece();
        }
    }
    public void AllCollectPiece()
    {
        player.gameObject.SetActive(false);
        playerCam.gameObject.SetActive(false);

        //ISCLEARPORTAL�� Ʈ��� ���� �� ������ ������ �� ��������
         SaveDataManager.Instance.IsClearPortal(SaveDataManager.Instance.CurrentStageNameData.worldName, SaveDataManager.Instance.CurrentStageNameData.stageCnt);
        if (isAllpiece) //������ �� ����
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
        player.gameObject.SetActive(true);
        player.gameObject.transform.position = playerSpawnpos.position;
        playerCam.gameObject.SetActive(true);

    }
}
