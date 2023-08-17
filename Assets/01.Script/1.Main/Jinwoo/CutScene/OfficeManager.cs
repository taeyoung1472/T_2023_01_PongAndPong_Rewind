using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class OfficeManager : MonoSingleTon<OfficeManager>
{
    [SerializeField] private CinemachineVirtualCamera talkCam;
    [SerializeField] private CinemachineVirtualCamera officeEnterCam;
    [SerializeField] private CinemachineVirtualCamera playerCam;

    [SerializeField] private Transform playerSpawnPos;
    [SerializeField] private Player player;
    [SerializeField] private GameObject playerTalk;

    [SerializeField] private Collider interactiveCheckCol;
    [SerializeField] private Image markImg;
    public bool isAnswerPhone = false;


    public void EnterOffice()
    {
        StartCoroutine(StartOffice());
    }
    public IEnumerator StartOffice()
    {
        isAnswerPhone = false;

        interactiveCheckCol.gameObject.SetActive(false);
        markImg.gameObject.SetActive(false);
        player.gameObject.SetActive(true);

        player.transform.position = playerSpawnPos.position;
        player.PlayerInput.enabled = false;
        player.ForceStop();

        yield return new WaitForSeconds(2f);

        player.PlayerInput.enabled = true;
        officeEnterCam.gameObject.SetActive(false);
        playerCam.gameObject?.SetActive(true);

        //3초 뒤에 마크이미지 나오면서 휴대폰 울림
        yield return new WaitForSeconds(3f);
        markImg.gameObject.SetActive(true); 
        interactiveCheckCol.gameObject.SetActive(true);
    }

    public void AnswerPhone()
    {
        isAnswerPhone = true;
        UIGetter.Instance.PushUIs();

        markImg.gameObject.SetActive(false);
        interactiveCheckCol.gameObject.SetActive(false);

        //phoneTalk.Play();
        playerTalk.SetActive(true);
        player.gameObject.SetActive(false);

        playerCam.Follow = null;
        playerCam.LookAt = null;

        playerCam.gameObject.SetActive(false);
        talkCam.gameObject.SetActive(true);
        ScenarioManager.Instance.StartAutoTalking();
        

    }

    public void EndPhone()
    {
        playerTalk.SetActive(false);
        talkCam.gameObject.SetActive(false);
    }
}
