using UnityEngine;

public class StageEndPoint : MonoBehaviour
{
    //[SerializeField] private StageAreaT curArea;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("?÷??̾? ?ص?");
            StageManager.Instance.curArea.IsClear = true;
        }
    }
}
