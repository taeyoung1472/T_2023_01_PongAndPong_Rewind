using UnityEngine;

public class GrabableObject : MonoBehaviour
{
    [Header("�� ��ġ, �÷��̾� ���ؿ��� ���� �����ʸ� ������ �� �ְԸ� �ϸ� ��")]
    [SerializeField]
    private Transform _wallPosition = null;
    [Header("�ö� ��ġ")]
    [SerializeField]
    private Transform _climbPosition = null;
    private Collider myCol;

    private void Awake()
    {
        myCol = GetComponent<Collider>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log($"A : {other.transform.position.y} B : {myCol.bounds.center.y + (myCol.bounds.size.y / 2f) - 2.0f}");
            float maxY = myCol.bounds.center.y + (myCol.bounds.size.y / 2f);
            if (other.transform.position.y <= maxY - 2.25f)
            {
                other.GetComponent<Player>().GetPlayerAction<PlayerWallGrab>(PlayerActionType.WallGrab).WallEnter(gameObject, _wallPosition.position);
            }
            else if(other.transform.position.y > maxY - 2.25f && other.transform.position.y <= maxY - 0.5f)
            {
                if (_climbPosition == null)
                    return;
                //other.GetComponent<Player>().GetPlayerAction<PlayerWallGrab>(PlayerActionType.WallGrab).WallEnter(gameObject, _wallPosition.position);
                other.GetComponent<Player>().GetPlayerAction<PlayerWallGrab>(PlayerActionType.WallGrab).WallClimb(_climbPosition.position, _wallPosition.position);
                // �� �ö󰡱�
            }
        }
    }
}