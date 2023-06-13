using UnityEngine;

public class GrabableObject : MonoBehaviour
{
    [Header("벽 위치, 플레이어 기준에서 왼쪽 오른쪽만 구별할 수 있게만 하면 됨")]
    [SerializeField]
    private Transform _wallPosition = null;
    [Header("올라갈 위치")]
    [SerializeField]
    private Transform _climbPosition = null;
    private Collider myCol;

    private Vector3 hitPosition = Vector3.zero;

    private void Awake()
    {
        myCol = GetComponent<Collider>();
    }

    private void Start()
    {
        if (_climbPosition == null)
            return;

        RaycastHit hit;
        bool isHit = Physics.Raycast(_climbPosition.position, Vector3.down, out hit, 100f, 1 << LayerMask.NameToLayer("Ground"));
        if (isHit)
            hitPosition = hit.point;
        else
            hitPosition = Vector3.zero;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player.PlayerActionCheck(PlayerActionType.WallGrab))
                return;

            float maxY = myCol.bounds.center.y + (myCol.bounds.size.y / 2f);
            if (other.transform.position.y <= maxY - 2.25f)
            {
                player.GetPlayerAction<PlayerWallGrab>(PlayerActionType.WallGrab).WallEnter(gameObject, _wallPosition.position);
            }
            else if (other.transform.position.y > maxY - 2.25f && other.transform.position.y <= maxY - 0.5f)
            {
                if (_climbPosition == null)
                    return;

                RaycastHit hit;
                bool isHit = Physics.Raycast(_climbPosition.position, Vector3.down, out hit, 100f, 1 << LayerMask.NameToLayer("Ground"));
                if (isHit)
                    hitPosition = hit.point;
                else
                    hitPosition = Vector3.zero;


                Vector3 playerStartPosition = player.transform.position;
                playerStartPosition.y = maxY - 1.65f;
                player.GetPlayerAction<PlayerWallGrab>(PlayerActionType.WallGrab).WallClimb(playerStartPosition, hitPosition, _wallPosition.position);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_climbPosition == null)
            return;
        RaycastHit hit;
        bool isHit = Physics.Raycast(_climbPosition.position, Vector3.down, out hit);
        if (isHit)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(_climbPosition.position, Vector3.down * hit.distance);
            Gizmos.DrawSphere(hit.point, 0.15f);
        }
    }
}