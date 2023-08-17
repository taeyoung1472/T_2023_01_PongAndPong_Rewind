using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GravityInverseGimmick : ControlAbleObjcet
{
    public DirectionType gravityDirState;
    public Dictionary<float, DirectionType> dirChangeDic = new Dictionary<float, DirectionType>();

    public Player player;
    public Player rewindPlayer;

    private DirectionType curDirection;

    private bool isRewind = false;

    private void PlayerGravitySet(DirectionType direction, Player player)
    {
        if (curDirection != direction)
        {
            AudioManager.PlayAudioRandPitch(SoundType.OnChangeGravity);
        }
        curDirection = direction;

        player.ForceStop();
        player.ColliderSet(PlayerColliderType.Normal);

        player.PlayerRenderer.FlipDirectionChange(direction, false);
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
                    player.ColliderSet(PlayerColliderType.Normal);
                    player.transform.position += (Vector3)(Utility.GetDirToVector(direction) * player.Col.height);
                }
                break;
            case DirectionType.Down:
                break;
        }
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
        }

    }

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
    }

    public override void ResetObject()
    {

    }
}
