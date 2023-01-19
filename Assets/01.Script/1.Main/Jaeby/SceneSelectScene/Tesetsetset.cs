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

        Instantiate(StageWorldSelectData.curStageWorld, null);
    }
}
