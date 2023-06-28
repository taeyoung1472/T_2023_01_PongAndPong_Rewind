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


    public Transform crossTrm;

    #region ¹°Ã¼
    public bool objIsOverlapping = false;
    public List<Transform> telObjList;
    #endregion
    private Collider col;

    [SerializeField] private bool isRight;

    [SerializeField] private float telValue = 2f;
    public override void Awake()
    {
        base.Awake();
        Init();

    }
    public override void Init()
    {
        col = GetComponent<Collider>();
        if (isRight)
        {
            crossTrm.position = transform.position + new Vector3(0.3f, 1.8f, 0);
        }
        else
        {
            crossTrm.position = transform.position + new Vector3(-0.3f, 1.8f, 0);
        }
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
            Vector3 centerPos = Vector3.zero;
            Vector3 diffVec = Vector3.zero;
            AudioManager.PlayAudioRandPitch(SoundType.OnPortal);
            foreach (Transform trm in telObjList)
            {
                Collider col = trm.GetComponent<Collider>();

                if (col is BoxCollider)
                {
                    centerPos = col.bounds.center;
                    diffVec = centerPos - trm.position;
                    diffVec.z = 0;
                    float offset = trm.GetComponent<Collider>().bounds.size.x;
                    if (isRight)
                    {
                        trm.position = (reciever.position + new Vector3(offset * telValue, 0, 0)) - diffVec;
                    }
                    else
                    {
                        trm.position = (reciever.position + new Vector3(-offset * telValue, 0, 0)) - diffVec;
                    }
                }
                else if (col is CapsuleCollider)
                {

                }
                else if (col is SphereCollider)
                {

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
            AudioManager.PlayAudioRandPitch(SoundType.OnPortal);
            Debug.Log(offset);
            playerIsOverlapping = false;
           // DeleteBuff();
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
            if (cross >= 0f)
            {
                playerIsOverlapping = true;
            Debug.Log("ÂïÇô¾ßµÊ¿ÜÀûÀÌÀÌ»óÇÔ");
                //  player.playerBuff.AddBuff(PlayerBuffType.Reverse);
            }

            Debug.Log("ÂïÇô¾ßµÊ");
        }

        if (other.gameObject.CompareTag("TelObj"))
        {
            Vector3 objToPlayer = playerTrm.position - crossTrm.position; //transform.position;
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
