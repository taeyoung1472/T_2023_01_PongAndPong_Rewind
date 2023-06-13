using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoTriggerNotify : MonoBehaviour
{
    [SerializeField] private string notifyKey;
    private bool isTriggered;

    public void OnTriggerEnter(Collider other)
    {
        if (isTriggered)
            return;

        if(other.gameObject.CompareTag("Player"))
        {
            isTriggered = true;
            TutorialPlayManager.Instance.PlayNotify(notifyKey);
        }
    }
}
