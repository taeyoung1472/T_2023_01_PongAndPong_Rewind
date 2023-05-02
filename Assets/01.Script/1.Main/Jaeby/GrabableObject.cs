using UnityEngine;

public class GrabableObject : MonoBehaviour
{
    [Header("벽 위치, 플레이어 기준에서 왼쪽 오른쪽만 구별할 수 있게만 하면 됨")]
    [SerializeField]
    private Transform _wallPosition = null;
    private Collider myCol;

    private void Awake()
    {
        myCol = GetComponent<Collider>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"A : {other.transform.position.y} B : {myCol.bounds.center.y + (myCol.bounds.size.y / 2f) - 2.0f}");
            if (other.transform.position.y < myCol.bounds.center.y + (myCol.bounds.size.y / 2f) - 2.0f)
            {
                other.GetComponent<Player>().GetPlayerAction<PlayerWallGrab>().WallEnter(gameObject, _wallPosition.position);
            }
        }
    }
}