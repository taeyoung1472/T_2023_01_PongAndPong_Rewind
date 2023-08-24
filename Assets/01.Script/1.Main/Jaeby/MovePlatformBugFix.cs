using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovePlatformBugFix : MonoBehaviour
{
    [ContextMenu("¹ö±× FIX")]
    public void Fix()
    {
        List<MovePlatform> platforms = new List<MovePlatform>();
        platforms = GameObject.FindObjectsOfType<MovePlatform>().ToList();
        foreach (MovePlatform platform in platforms)
        {
            if(platform != null)
            {
                if(platform.ParentTrm ==  null)
                {
                    if(platform.transform.localScale != Vector3.one)
                    {
                        GameObject modelParentTrm = new GameObject("modelParentTrm");
                        modelParentTrm.transform.SetParent(platform.transform.parent);
                        platform.ParentTrm = modelParentTrm.transform;
                    }
                }
                else
                {
                    if (platform.ParentTrm.localScale != Vector3.one)
                    {
                        GameObject modelParentTrm = new GameObject("modelParentTrm");
                        modelParentTrm.transform.SetParent(platform.ParentTrm.parent);
                        platform.ParentTrm = modelParentTrm.transform;
                    }
                }
            }
        }
    }
}
