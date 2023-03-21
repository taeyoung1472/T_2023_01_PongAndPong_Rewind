using UnityEngine;

public class StageEndPoint : MonoBehaviour
{
    //[SerializeField] private StageAreaT curArea;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StageManager.Instance.currentStage.curArea.AreaData.isAreaClear= true;
            other.GetComponent<Player>().enabled = false;
            other.GetComponent<PlayerInput>().enabled = false;
            other.GetComponent<CharacterController>().enabled = false;
        }
    }
}
