using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringGimmick : MonoBehaviour
{
    private Collider _col = null;
    public float rayDistance = 1f;
    public bool isJump = false;
    public int jumpPower;

    [SerializeField]
    private int _maxDeceleration = 4;
    public Dictionary<Collider, int> decelerationColDic = new Dictionary<Collider, int>();

    private void Awake()
    {
        _col = GetComponent<Collider>();
    }
    private void Update()
    {
        ShootUpBoxCast();
       // ShootInfi();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            float time = collision.collider.GetComponent<StayTimeChecker>().StayTime;

            float weight = collision.collider.GetComponent<ObjWeight>().so.weight;

            float ratio = time * weight;

        }
    }
    public void ShootInfi()
    {
        RaycastHit hit;

        Vector3 boxCenter = _col.bounds.center;
        Vector3 halfExtents = _col.bounds.extents;

        Physics.BoxCast(boxCenter, new Vector3(halfExtents.x, halfExtents.y, halfExtents.z), transform.up, out hit, transform.rotation, Mathf.Infinity);

        if (hit.collider != null)
        {

        }
        else
        {
            decelerationColDic.Remove(hit.collider);
        }
    }

    //�ڽ�ĳ��Ʈ�� ���� ĳ���Ϳ� ���ͤ��̾��� �� ü���ð��� �ƴ°� ����;��ϴ°���
    public void ShootUpBoxCast()
    {
        RaycastHit hit;

        Vector3 boxCenter = _col.bounds.center;
        Vector3 halfExtents = _col.bounds.extents;

        Physics.BoxCast(boxCenter, new Vector3(halfExtents.x, halfExtents.y, halfExtents.z), transform.up, out hit, transform.rotation, rayDistance);

        if (hit.collider != null)
        {
          

            if (!decelerationColDic.ContainsKey(hit.collider))
            {
                decelerationColDic.Add(hit.collider, 0);
                decelerationColDic[hit.collider] = _maxDeceleration;
            }

            float time = hit.collider.GetComponent<StayTimeChecker>().StayTime;

            float weight = hit.collider.GetComponent<ObjWeight>().so.weight;

            float ratio = time * weight; //��Ƽ���� ũ�� ����cnt�� ũ�� �����䰡 ������ ���� cnt�� �۾�

            jumpPower = decelerationColDic[hit.collider] * (int)ratio;
            // 4 2 1 0

            // Debug.Log(decelerationColDic[hit.collider] + "      " + ratio);
            if (isJump == false)
            {
                if (decelerationColDic[hit.collider] <= 0)
                {
                    return;
                }
                decelerationColDic[hit.collider] /= 2;
                isJump = true;
            }

            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.GetComponent<PlayerJump>().ForceJump(Vector3.up, jumpPower);
            }
            else
            {
                hit.collider.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }

        }
        else
        {
            isJump = false;

            foreach (var exitCol in decelerationColDic)
            {
                float X = exitCol.Key.gameObject.transform.position.x; //��ü��ġ 

                float boxX = halfExtents.x; // �ڽ� ĳ��Ʈ ���� ����
                if (X > boxX + transform.position.x || X < transform.position.x - boxX)
                {
                    decelerationColDic.Remove(exitCol.Key);
                    Debug.Log("����?");
                }
            }
        }

    
    }

}
