using UnityEngine;

public class TriggerNotify : MonoBehaviour
{
    [SerializeField] private string notifyText;
    bool isTriggerd = false;
    public void OnTriggerEnter(Collider other)
    {
        if (isTriggerd)
            return;

        if (other.GetComponent<Player>())
        {
            isTriggerd = true;
            NotifyManager.Instance.Notify(notifyText);
        }
    }
}
