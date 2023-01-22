using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TransformInfo : MonoBehaviour
{
    public int index;
    private void Start()
    {
        try
        {
            SaveManager.Instance.transformList.Add(this);
        }
        catch
        {
            //  Unity Life Cycle Issuse
        }
    }

    private void OnDestroy()
    {
        try
        {
            SaveManager.Instance.transformList.Remove(this);
        }
        catch
        {
            //  Unity Life Cycle Issuse
        }
    }
}

