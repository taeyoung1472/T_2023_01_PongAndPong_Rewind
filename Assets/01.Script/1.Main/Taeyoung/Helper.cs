using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    void Start()
    {
        RewindManager.Instance.InitPlay += () => gameObject.SetActive(false);
    }
}
