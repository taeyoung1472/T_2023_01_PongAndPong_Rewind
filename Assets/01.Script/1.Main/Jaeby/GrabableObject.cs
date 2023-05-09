using UnityEngine;

public class GrabableObject : MonoBehaviour
{
    [Header("�� ��ġ, �÷��̾� ���ؿ��� ���� �����ʸ� ������ �� �ְԸ� �ϸ� ��")]
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