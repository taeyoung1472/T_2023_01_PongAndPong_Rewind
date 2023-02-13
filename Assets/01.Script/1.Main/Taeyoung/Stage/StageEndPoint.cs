using UnityEngine;

public class StageEndPoint : MonoBehaviour
{
    [SerializeField] private StageArea curArea;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            curArea.IsClear = true;
        }
    }
}
