using UnityEngine;

public class StageEndPoint : MonoBehaviour
{
    //[SerializeField] private StageAreaT curArea;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 앤드");
            StageTestManager.Instance.curArea.IsClear = true;
        }
    }
}
