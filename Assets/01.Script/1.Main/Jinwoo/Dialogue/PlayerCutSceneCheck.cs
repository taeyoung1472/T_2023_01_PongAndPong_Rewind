using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCutSceneCheck : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("ElevatorCutScene"))
        {
            Debug.Log("�ù߷þ�");
            CutSceneManager.Instance.StartElevatorCutScene();
        }
    }

}

