using System.Collections;
using UnityEngine;

public class GravityInverseGimmick : GimmickObject
{
    public override void Init()
    {
    }

    public DirectionType gravityDirState;

    [SerializeField]
    private float _gravityScale = 0.8f;
    [SerializeField]
    private float _coolTime = 0.2f;
    private bool _locked = false;
    [SerializeField]
    private LayerMask _groundMask = 0;

    RaycastHit hit;

    private void OnTriggerEnter(Collider other)
    {
        if (_locked)
            return;

        if (other.CompareTag("Player"))
        {
            if (player == null)
                player = other.GetComponent<Player>();

            player.GravityModule.GravityScale = _gravityScale;
            PlayerGravitySet(gravityDirState);
        }
        /*if (other.gameObject.TryGetComponent<GravityGimmickObject>
            (out GravityGimmickObject gravityGimmick))
        {
            gravityGimmick.GravityDir = Utility.GetDirToVector(gravityDirState) * 9.8f;
            gravityGimmick.GravityScale = _gravityScale;
        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        if (_locked)
            return;

        if (player == null)
        {
            return;
        }
        player.GravityModule.GravityScale = player.GravityModule.OriginGravityScale;
        PlayerGravitySet(DirectionType.Down);
        player = null;
        //module.GravityScale = module.OriginGravityScale;

        /*if (other.gameObject.TryGetComponent<GravityGimmickObject>
                  (out GravityGimmickObject gravityGimmick))
        {
            gravityGimmick.GravityDir = Utility.GetDirToVector(FlipDirection.Down) * 9.8f;
            gravityGimmick.GravityScale = gravityGimmick.OrignGravityScale;
        }*/
    }

    private void PlayerGravitySet(DirectionType direction)
    {
        player.ForceStop();
        player.ColliderSet(PlayerColliderType.Normal);
        CapsuleCollider col = player.Col;
        if ((gravityDirState == DirectionType.Left || gravityDirState == DirectionType.Right))
        {
            Vector3 newPos = Vector3.zero;
            if (RayCheck(Vector3.up, col))
            {
                newPos = hit.point;
                newPos.y -= col.height * 1.1f;
                player.transform.position = newPos;
            }
        }
        else
        {
            Vector3 newPos = Vector3.zero;
            newPos = player.transform.position + player.transform.up * col.height;
            player.transform.position = newPos;
        }
        player.PlayerRenderer.flipDirection = direction;
        player.GravityModule.GravityDir = Utility.GetDirToVector(direction) * 9.8f;
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
}
