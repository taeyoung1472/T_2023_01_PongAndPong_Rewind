using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingCheck : MonoBehaviour
{
    [SerializeField] private bool isTalk = false;

    private void Start()
    {
        isTalk = false;
    }
    public void SetIsTalk(bool isCheck)
    {
        isTalk = isCheck;
    }
    public bool GetIsTalk()
    {
        return isTalk;
    }

}
