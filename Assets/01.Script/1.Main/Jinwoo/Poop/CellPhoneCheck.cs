using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellPhoneCheck : Interact
{
    protected override void ChildInteractEnd()
    {
    }

    protected override void ChildInteractStart()
    {
        Debug.Log("��ȭ����");
        if (OfficeManager.Instance != null)
            OfficeManager.Instance.AnswerPhone();
        //if (OfficeCutSceneManager.Instance != null)
        //    OfficeCutSceneManager.Instance.AnswerCellPhone();
        //else if (OfficeCutScene2.Instance != null)
        //    OfficeCutScene2.Instance.AnswerCellPhone();

    }
}
