using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GravityInverseGimmick : GimmickObject
{
    public override void Init()
    {
    }
    Player player = null;

    public DirectionType gravityDirState;

    [SerializeField]
    private float _gravityScale = 0.8f;
    [SerializeField]
    private float _coolTime = 0.2f;
    private bool _locked = false;
    [SerializeField]
    private LayerMask _groundMask = 0;

    private void OnTriggerEnter(Collider other)
    {
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
        CapsuleCollider col = player.Col;
        RaycastHit hit;
        bool result = Physics.Raycast(player.transform.position, player.transform.up * -1f, out hit, col.height, _groundMask);
        if(result)
        {
        }
        if (gravityDirState == DirectionType.Left || gravityDirState == DirectionType.Right)
        {
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
}
