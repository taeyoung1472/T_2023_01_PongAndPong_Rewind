using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GravityInverseGimmick : ControlAbleObjcet
{
    public DirectionType gravityDirState;
    public Dictionary<float, DirectionType> dirChangeDic = new Dictionary<float, DirectionType>();
    //버튼을 밟을까 바뀔방향이 
    [SerializeField]
    private float _gravityScale = 0.8f;
    [SerializeField]
    private float _coolTime = 0.2f;
    private bool _locked = false;
    [SerializeField]
    private LayerMask _groundMask = 0;

    public Player player;
    public Player rewindPlayer;

    RaycastHit hit;

    private float timer = 0f;

    [SerializeField] private float moveValue = 2f;

    private void OnTriggerEnter(Collider other)
    {
        //if (_locked)
        //    return;

        //if (other.CompareTag("Player"))
        //{
        //    if (player == null)
        //        player = other.GetComponent<Player>();

        //    player.GravityModule.GravityScale = _gravityScale;
        //    PlayerGravitySet(gravityDirState, rewindPlayer);
        //}
        /*if (other.gameObject.TryGetComponent<GravityGimmickObject>
            (out GravityGimmickObject gravityGimmick))
        {
            gravityGimmick.GravityDir = Utility.GetDirToVector(gravityDirState) * 9.8f;
            gravityGimmick.GravityScale = _gravityScale;
        }*/
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (_locked)
    //        return;

    //    if (player == null)
    //    {
    //        return;
    //    }
    //    player.GravityModule.GravityScale = player.GravityModule.OriginGravityScale;
    //    PlayerGravitySet(DirectionType.Down);
    //    player = null;
    //    //module.GravityScale = module.OriginGravityScale;

    //    /*if (other.gameObject.TryGetComponent<GravityGimmickObject>
    //              (out GravityGimmickObject gravityGimmick))
    //    {
    //        gravityGimmick.GravityDir = Utility.GetDirToVector(FlipDirection.Down) * 9.8f;
    //        gravityGimmick.GravityScale = gravityGimmick.OrignGravityScale;
    //    }*/
    //}


    private void PlayerGravitySet(DirectionType direction, Player player)
    {
        player.ForceStop();
        player.ColliderSet(PlayerColliderType.Normal);

        player.PlayerRenderer.flipDirection = direction;
        player.GravityModule.GravityDir = Utility.GetDirToVector(direction) * 9.8f;

        CapsuleCollider col = player.Col;
        Vector3 newPos = Vector3.zero;
        switch (direction)
        {
            case DirectionType.Left:
                break;
            case DirectionType.Right:
                break;
            case DirectionType.Up:
                if (isRewind)
                {
                    Debug.Log("너 재비니?");
                    player.ColliderSet(PlayerColliderType.Normal);
                    player.transform.Translate(Vector2.down * player.Col.height);
                }
                break;
            case DirectionType.Down:
                if (isRewind)
                {
                    //player.ColliderSet(PlayerColliderType.Normal);
                    //player.transform.Translate(Vector2.down * player.Col.height);
                }
                break;
        }
    }

    private bool RayCheck(Vector3 dir, CapsuleCollider playerCol)
    {
        if (player == null)
            return false;
        return Physics.Raycast(player.transform.position, dir, out hit, playerCol.height, _groundMask);
    }

    private IEnumerator CooltimeCoroutine()
    {
        _locked = true;
        yield return new WaitForSeconds(_coolTime);
        _locked = false;
    }

    public override void Control(ControlType controlType, bool isLever, Player player, DirectionType dirType)
    {
        curControlType = controlType;

        switch (controlType)
        {
            case ControlType.Control:
                if (rewindPlayer != null)
                {
                    gravityDirState = dirType;
                    PlayerGravitySet(gravityDirState, rewindPlayer);
                }
                else
                {
                    PlayerGravitySet(dirType, player);
                }
                break;
            case ControlType.None:
                // PlayerGravitySet(DirectionType.Down);
                break;
            case ControlType.ReberseControl:
                break;
        }
    }
    private void Awake()
    {
        if (RewindManager.Instance)
        {
            RewindManager.Instance.InitRewind += InitOnRewind;
            RewindManager.Instance.InitPlay += InitOnPlay;
            //RewindManager.Instance.RestartPlay += InitOnPlay;
        }

    }
    public bool isRewind = false;
    private void InitOnPlay()
    {
        rewindPlayer = null;
        isRewind = false;
        StopAllCoroutines();
        dirChangeDic.Clear();
        dirChangeDic.Add(0, gravityDirState);
        if (player == null)
        {
            player = FindObjectOfType<Player>();
            Debug.Log(player);
        }
    }

    private void InitOnRewind()
    {
        player = null;
        isRewind = true;
        if (rewindPlayer == null)
        {
            rewindPlayer = FindObjectOfType<Player>();
            Debug.Log(gravityDirState);
            StartCoroutine(DirChangeCo());
        }
    }
    private IEnumerator DirChangeCo()
    {
        yield return null;
        dirChangeDic = dirChangeDic.OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

        float beforeKey = 11f;
        foreach (var item in dirChangeDic)
        {
            PlayerGravitySet(item.Value, rewindPlayer);
            yield return new WaitForSeconds(beforeKey - item.Key);
            beforeKey = item.Key;
        }


        //dirChangeDic.Clear();

            //Debug.Log("아이템의 키:"+ item.Key + "아이템의 밸류:" + item.Value);
            //Debug.Log("storage = " + storage);
            //yield return new WaitForSeconds(item.Key - storage); //스테이지 시간
            //storage += item.Key;
            //PlayerGravitySet(item.Value, rewindPlayer);
            //Debug.Log("잘 돌았니?" + rewindPlayer.PlayerRenderer.flipDirection);

        //foreach마지막이 실행이안됨
        //한번돌고 실행하고 딕셔너리를 2번돈다  
        //딕셔너리가 빌떄까지 
        //한번사용 하면 딕셔너리
    }
}
