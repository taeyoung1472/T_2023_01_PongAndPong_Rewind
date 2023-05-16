using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjManager : MonoBehaviour
{
    public static GameObject ObjInstantiate(GameObject obj, Vector3 pos, Quaternion rot = default(Quaternion))
    {
        return Instantiate(obj, pos, rot);
    }
    public static void ObjInstantiate(GameObject obj)
    {
        Destroy(obj);
    }
}
