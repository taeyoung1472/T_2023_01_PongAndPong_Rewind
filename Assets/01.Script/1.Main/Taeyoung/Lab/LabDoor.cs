using UnityEngine;

public class LabDoor : MonoBehaviour
{
    private Transform playerTrans;

    [SerializeField] private float openMin;
    [SerializeField] private float openStart;

    [SerializeField] private Transform[] pivots;

    private void Awake()
    {
        playerTrans = FindObjectOfType<Player>().transform;
    }

    void Update()
    {
        float dist = Mathf.Abs(transform.position.x - playerTrans.position.x);

        foreach (var pivot in pivots)
        {
            pivot.transform.localScale = new Vector3(1, 1, dist < openMin ? 0 : dist < (openMin + openStart) ? (dist - openMin) / openStart : 1);
        }
    }
}
