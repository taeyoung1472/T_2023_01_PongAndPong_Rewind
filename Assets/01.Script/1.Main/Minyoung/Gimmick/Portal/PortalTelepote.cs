using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class PortalTelepote : MonoBehaviour
{
    public Transform playerTrm;
    public Transform reciverTrm;

    private Collider _col;
    private bool isPlayerOverlapping;
    public float rayDistance = 1f;
    public bool isFront;
    public CinemachineVirtualCamera Vcam;

    public Transform renderTrm;

    public bool isRight = true;
    float dotProduct;


    public List<Material> playerSkinedMatList = new List<Material>();



    private void Awake()
    {
        _col = GetComponent<Collider>();
        playerSkinedMatList.AddRange(playerTrm.Find("AgentRenderer/Model/man-mafia_Rig/man-mafia_Rig").GetComponentInChildren<SkinnedMeshRenderer>().materials);
    }
    private void Update()
    {

        if (isPlayerOverlapping)
        {
            playerTrm.GetComponent<CharacterController>().enabled = false;
            Vector3 portalToPlayer = playerTrm.position - transform.position;

            if (isRight)
            {
                dotProduct = Vector3.Dot(transform.right * 1f, portalToPlayer);

                foreach (var mat in playerSkinedMatList)
                {
                    mat.SetVector("_DissolveDirection", new Vector3(1, 0, 0));
                }
            }
            else
            {
                dotProduct = Vector3.Dot(transform.right * -1f, portalToPlayer);

                foreach (var mat in playerSkinedMatList)
                {
                    mat.SetVector("_DissolveDirection", new Vector3(-1, 0, 0));
                }
            }


            Debug.Log(dotProduct);
            if (dotProduct > 0f)
            {

                foreach (var mat in playerSkinedMatList)
                {
                    mat.SetFloat("_Dissolve", 1f);
                }

                float rotationDiff = -Quaternion.Angle(transform.rotation, reciverTrm.rotation);
                rotationDiff += 180;
                playerTrm.Rotate(Vector3.up, rotationDiff);

                Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
                playerTrm.position = reciverTrm.position + positionOffset + (Vector3.right * (positionOffset.x * 0.5f));

                StartCoroutine(Fade());

                isPlayerOverlapping = false;
            }
        }
        else
        {
            playerTrm.GetComponent<CharacterController>().enabled = true;
        }
    }
    IEnumerator Fade()
    {
       
        yield return new WaitForSeconds(0.5f);

        foreach (var mat in playerSkinedMatList)
        {
            DoFade(0.5f, 0, 1f, mat);
        }
    }

    public void DoFade(float start, float dest, float time, Material dissolveMat)
    {
        DOTween.To(() => start, x => { start = x; dissolveMat.SetFloat("_Dissolve", start); }, dest, time).SetUpdate(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("잉이이");
        if (other.CompareTag("GimmickPlayerCol"))
        {
            Debug.Log("플레이어와 충돌함");
            isPlayerOverlapping = true;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Gimmick"))
        {
            TelObj(other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GimmickPlayerCol"))
        {
            isPlayerOverlapping = false;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Gimmick"))
        {
            Debug.Log("물체와 충돌함");
        }
    }
    private void TelObj(Transform colTrm)
    {
        Debug.Log("텔오비지");
        Vector3 portalToObj = colTrm.position - transform.position;

        if (isRight)
        {
            dotProduct = Vector3.Dot(transform.right * 1f, portalToObj);
        }
        else
        {
            dotProduct = Vector3.Dot(transform.right * -1f, portalToObj);
        }

        if (dotProduct > 0f)
        {
            colTrm.position = reciverTrm.position + new Vector3(5, 0, 0);
        }
    }
}
