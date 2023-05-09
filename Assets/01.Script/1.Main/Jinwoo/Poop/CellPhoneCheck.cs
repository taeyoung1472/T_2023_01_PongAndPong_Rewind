using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellPhoneCheck : MonoBehaviour
{
    public Transform _interactUIPos;
    public Sprite _interactSprite;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIGetter.Instance.GetInteractUI(_interactUIPos.position, _interactSprite, KeyManager.keys[InputType.Interact]);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("전화받음");
                if(OfficeCutSceneManager.Instance != null)
                    OfficeCutSceneManager.Instance.AnswerCellPhone();
                else if(OfficeCutScene2.Instance != null) 
                    OfficeCutScene2.Instance.AnswerCellPhone();

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIGetter.Instance.PushUIs();
        }
    }
}
