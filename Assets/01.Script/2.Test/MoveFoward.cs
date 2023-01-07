using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFoward : MonoBehaviour
{
    void Update()
    {
        if(!RewindManager.Instance.IsRewinding && !RewindManager.Instance.IsEnd)
        {
            transform.position += Vector3.forward * 2.5f * Time.deltaTime;
            transform.Rotate(Vector3.up * 60 * Time.deltaTime);
        }
    }
}
