using UnityEngine;

public class EndPoint : MonoBehaviour
{
    bool isActive = false;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActive)
        {
            Time.timeScale = 5;
            EndManager.Instance.IsClear = true;
            isActive = true;
        }
    }
}
