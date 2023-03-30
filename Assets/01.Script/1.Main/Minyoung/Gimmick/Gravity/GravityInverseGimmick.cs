using UnityEngine;

public class GravityInverseGimmick : GimmickObject
{
    public override void Init()
    {
    }
    Player player = null;

    GravityModule module;

    public enum GravityDirState
    {
        Up,
        Down,
        Left,
        Right,
    }

    public GravityDirState gravityDirState;


    private void OnTriggerStay(Collider other)
    {
        /*
        if (other.CompareTag("GimmickPlayerCol"))
        {
            player.GravityModule.GravityScale = 0.2f;

            switch (gravityDirState)
            {
                case GravityDirState.Up:
                    module.GravityDir = new Vector3(0f, 9.8f, 0f);
                    player.transform.rotation = Quaternion.Euler(180f, player.transform.rotation.y, player.transform.rotation.z);
                    break;
                case GravityDirState.Down:
                    module.GravityDir = new Vector3(0f, -9.8f, 0f);
                    player.transform.rotation = Quaternion.Euler(0f, player.transform.rotation.y, player.transform.rotation.z); 
                    break;
                case GravityDirState.Left:
                    module.GravityDir = new Vector3(-9.8f, 0f, 0f);
                    player.transform.rotation = Quaternion.Euler(player.transform.rotation.x, player.transform.rotation.y, -90f);
                    break;
                case GravityDirState.Right:
                    module.GravityDir = new Vector3(9.8f, 0f, 0f);
                    player.transform.rotation = Quaternion.Euler(player.transform.rotation.x, player.transform.rotation.y, 90f);
                    break;
            }

        }

        if (other.gameObject.TryGetComponent<GravityGimmickObject>(out GravityGimmickObject gravityGimmick))
        {
           // gravityGimmick.GravityScale = 0.5f;
            switch (gravityDirState)
            {
                case GravityDirState.Up:
                    gravityGimmick.GravityVec = new Vector3(0, 9.8f, 0);
                    break;
                case GravityDirState.Down:
                    gravityGimmick.GravityVec = new Vector3(0, -9.8f, 0);
                    break;
                case GravityDirState.Left:
                    gravityGimmick.GravityVec = new Vector3(-9.8f, 0, 0);
                    break;
                case GravityDirState.Right:
                    gravityGimmick.GravityVec = new Vector3(9.8f, 0, 0);
                    break;
            }
        }
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GimmickPlayerCol"))
        {
            if (module == null)
                module = other.GetComponentInParent<GravityModule>();
            if (player == null)
                player = other.GetComponentInParent<Player>();

            player.PlayerRenderer.flipDirection = FlipDirection.Up;
            module.GravityScale = 0.8f;
            module.GravityDir = new Vector3(0f, 9.8f, 0f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (module == null)
        {
            return;
        }
        module.GravityScale = module.OriginGravityScale;
        module.GravityDir = new Vector3(0f, -9.8f, 0f);
        player.PlayerRenderer.flipDirection = FlipDirection.Down;


    }
}
