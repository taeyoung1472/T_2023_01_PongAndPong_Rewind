using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tesetsetset : MonoBehaviour
{
    private void Awake()
    {
        if(StageWorldSelectData.curStageWorld == null)
        {
            Debug.LogWarning("CurStaegeWorld is null !!");
            return;
        }

        // TODO 가져오기
        Instantiate(StageWorldSelectData.curStageWorld, null);
    }
}
