using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePlateGimmick : MonoBehaviour
{
    private IEnumerator coroutine;
    [SerializeField] private float destroyTime = 0f;
    private float basicTime;
    private void Awake()
    {
        coroutine = DestoryObjTime(destroyTime);
        basicTime = destroyTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GimmickPlayerCol"))
        {
            //StartCoroutine(coroutine);
            //Debug.Log("�ڷ�ƾ����");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("GimmickPlayerCol"))
        {
            destroyTime -= Time.deltaTime;
            if (destroyTime <= 0)
            {
                Debug.Log("����");
                Destroy(gameObject);
            }
        }

     
    }
    private void OnTriggerExit(Collider other)
    {
        destroyTime = basicTime;
        if (coroutine != null)
        {
            //StopCoroutine(coroutine);
            //Debug.Log("��ž�ڷ�ƾ");
        }
       // print(destroyTime);
    }

    IEnumerator DestoryObjTime(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("����");
        Destroy(gameObject);
    }
}
