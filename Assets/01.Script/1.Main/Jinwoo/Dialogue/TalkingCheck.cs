using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingCheck : MonoBehaviour
{
    [SerializeField] private bool isTalk = false;
    [SerializeField] private GameObject myCol;
    private void Awake()
    {
        myCol = transform.Find("InteractCollider").gameObject;
    }
    private void Start()
    {
        isTalk = false;
    }

    //����� OnInteractEnd() �̺�Ʈ �ȿ� �־���
    public void SetIsTalk(bool isCheck)
    {
        isTalk = isCheck;
        DisableTalk();
    }
    public bool GetIsTalk()
    {
        return isTalk;
    }

    public void DisableTalk()
    {
        myCol.SetActive(false);
    }

}
