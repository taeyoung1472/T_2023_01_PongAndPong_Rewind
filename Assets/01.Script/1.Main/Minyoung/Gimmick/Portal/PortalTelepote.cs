using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class PortalTelepote : MonoBehaviour
{
    public Transform player;
    public Transform reciever;

    public  bool playerIsOverlapping = false;

    public bool isRight = false;
    float dotProduct = 0f;

    void Update()
    {
        PlayerTelPo();
    }
    void PlayerTelPo()
    {
        if (playerIsOverlapping)
        {
            Vector3 portalToPlayer = player.position - transform.position;
            if (isRight)
            {
                dotProduct = Vector3.Dot(transform.right, portalToPlayer);
                Debug.Log(dotProduct);
            }
            else
            {
                dotProduct = Vector3.Dot(transform.right * -1f, portalToPlayer);
                Debug.Log(dotProduct);
            }

            if (dotProduct > 0f)
            {
                player.position = reciever.position;
                playerIsOverlapping = false;
            }
        }
    }    

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsOverlapping = true;
        }
     
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsOverlapping = false;
        }
   
    }
    //public Transform playerTrm;
    //public Transform reciverTrm;

    //private Collider _col;
    //private bool isPlayerOverlapping;
    //public float rayDistance = 1f;
    //public bool isFront;
    //public CinemachineVirtualCamera Vcam;

    //public Transform renderTrm;

    //public bool isRight = true;
    //float dotProduct;


    //public List<Material> playerSkinedMatList = new List<Material>();

    //Sequence _seq = null;


    //private void Awake()
    //{
    //    _col = GetComponent<Collider>();
    //    //playerSkinedMatList.AddRange(playerTrm.Find("AgentRenderer/Model/man-mafia_Rig/man-mafia_Rig").GetComponentInChildren<SkinnedMeshRenderer>().materials);
    //}
    //private void Update()
    //{

    //    if (isPlayerOverlapping)
    //    {
    //        playerTrm.GetComponent<CharacterController>().enabled = false;
    //        Vector3 portalToPlayer = playerTrm.position - transform.position;

    //        if (isRight)
    //        {
    //            dotProduct = Vector3.Dot(transform.right * 1f, portalToPlayer);

    //            foreach (var mat in playerSkinedMatList)
    //            {
    //                mat.SetVector("_DissolveDirection", new Vector3(1, 0, 0));
    //            }
    //        }
    //        else
    //        {
    //            dotProduct = Vector3.Dot(transform.right * -1f, portalToPlayer);

    //            foreach (var mat in playerSkinedMatList)
    //            {
    //                mat.SetVector("_DissolveDirection", new Vector3(-1, 0, 0));
    //            }
    //        }
    //        if (dotProduct > 0f)
    //        {
    //            float rotationDiff = -Quaternion.Angle(transform.rotation, reciverTrm.rotation);
    //            rotationDiff += 180;
    //            playerTrm.Rotate(Vector3.up, rotationDiff);

    //            Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
    //            playerTrm.position = reciverTrm.position + positionOffset + (Vector3.right * (positionOffset.x * 0.5f));

    //            DissolveSeq();

    //            isPlayerOverlapping = false;
    //        }
    //    }
    //    else
    //    {
    //        playerTrm.GetComponent<CharacterController>().enabled = true;
    //    }
    //}

    //private void DissolveSeq()
    //{
    //    if (_seq != null)
    //        _seq.Kill();
    //    _seq = DOTween.Sequence();
    //    //_seq.Append(DoFade(1f, 0f, 2f, playerSkinedMatList[0]));
    //    for(int i = 0; i < playerSkinedMatList.Count; i++)
    //        _seq.Join(DoFade(1f, 0f, 2f, playerSkinedMatList[i]));
    //}

    //public Tween DoFade(float start, float dest, float time, Material dissolveMat)
    //{
    //    return DOTween.To(() => start, x => { start = x; dissolveMat.SetFloat("_Dissolve", start); }, dest, time);
    //}
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("GimmickPlayerCol"))
    //    {
    //        Debug.Log("플레이어와 충돌함");
    //        isPlayerOverlapping = true;
    //    }
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Gimmick"))
    //    {
    //        TelObj(other.transform);
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("GimmickPlayerCol"))
    //    {
    //        isPlayerOverlapping = false;
    //    }
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Gimmick"))
    //    {
    //        Debug.Log("물체와 충돌함");
    //    }
    //}
    //private void TelObj(Transform colTrm)
    //{
    //    Debug.Log("텔오비지");
    //    Vector3 portalToObj = colTrm.position - transform.position;

    //    if (isRight)
    //    {
    //        dotProduct = Vector3.Dot(transform.right * 1f, portalToObj);
    //    }
    //    else
    //    {
    //        dotProduct = Vector3.Dot(transform.right * -1f, portalToObj);
    //    }

    //    if (dotProduct > 0f)
    //    {
    //        colTrm.position = reciverTrm.position + new Vector3(5, 0, 0);
    //    }
    //}
}
