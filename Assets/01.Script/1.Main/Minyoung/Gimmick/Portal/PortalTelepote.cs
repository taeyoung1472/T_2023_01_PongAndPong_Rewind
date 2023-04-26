using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class PortalTelepote : GimmickObject
{
    #region Player
    public Transform playerTrm;
    public  bool playerIsOverlapping = false;
    #endregion

    public float cross;
    public Transform reciever;

    #region 물체
    public bool objIsOverlapping = false;
    public List<Transform> telObjList;
    #endregion
    private Collider col;

    [SerializeField] private bool isRight;
   
    public override void Awake()
    {
        base.Awake();
        Init();
    }
    public override void Init()
    {
        col = GetComponent<Collider>();
    }
    public override void InitOnPlay()
    {
        base.InitOnPlay();
        playerTrm = null;
        if (playerTrm == null)
        {
            playerTrm = FindObjectOfType<Player>().transform;
        }
        cross = 0f;
    }
    public override void InitOnRewind()
    {
        base.InitOnRewind();
    }
    void Update()
    {
        if (isRewind)
        {
            return;
        }
        PlayerTelPo();
        TelPoObj();
    }
    void TelPoObj()
    {
        if (objIsOverlapping)
        {
            Vector3 centerPos = col.bounds.center;
            Debug.Log("CenterPos" + centerPos);
            foreach (Transform trm in telObjList)
            {
                Vector3 diffVec = centerPos - trm.position;
                diffVec.z = 0;
                Debug.Log(diffVec);
                float offset = trm.GetComponent<Collider>().bounds.size.x;
                Debug.Log(offset);
                if (isRight)
                {
                    trm.position = (reciever.position + new Vector3(offset * 2f, 0, 0)) - diffVec;
                }
                else
                {
                    trm.position = (reciever.position + new Vector3(-offset * 2f, 0, 0)) - diffVec;
                }
            }
        }

    }
    void PlayerTelPo()
    {
        if (playerIsOverlapping)
        {
            Vector3 offset = new Vector3(player.GetComponent<CapsuleCollider>().radius * 2.5f, 0, 0);
            if (isRight)
            {
                playerTrm.position = reciever.position + offset;
            }
            else
            {
                playerTrm.position = reciever.position + -offset;
            }
            Debug.Log(offset);
            playerIsOverlapping = false;
            DeleteBuff();
            //Invoke("DeleteBuff", 0.1f);
        }
    }
    
    public void DeleteBuff()
    {
        if (player == null)
            return;

        player.playerBuff.DeleteBuff(PlayerBuffType.Reverse);
    }
    void OnTriggerEnter(Collider other)
    {
        if (isRewind)
        {
            return;
        }

        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 portalToPlayer = playerTrm.position - transform.position;
            Vector3 crossVec = Vector3.Cross(transform.forward, portalToPlayer);
            cross = crossVec.y;
            Debug.Log("작동 잘하나요" + cross);
            if (cross >= 0f)
            {
                playerIsOverlapping = true;
                Debug.Log(player);
                Debug.Log(player.playerBuff);
                player.playerBuff.AddBuff(PlayerBuffType.Reverse);
            }
        }

        if (other.gameObject.CompareTag("TelObj"))
        {
            Vector3 objToPlayer = playerTrm.position - transform.position;
            Vector3 crossVec = Vector3.Cross(transform.forward, objToPlayer);
            cross = crossVec.y;
            if (cross >= 0f)
            {
                telObjList.Add(other.gameObject.transform);
                objIsOverlapping = true;
            }
    
        }     
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsOverlapping = false;
        }
        if (other.gameObject.CompareTag("TelObj"))
        {
            telObjList.Remove(other.gameObject.transform);
            objIsOverlapping = false;
        }

    }
}
