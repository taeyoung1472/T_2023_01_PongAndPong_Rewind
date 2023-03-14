using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringGimmick : MonoBehaviour
{
    private Collider _col = null;
    public float rayDistance = 1f;
    private bool isJump = false;
    private float jumpPower;
    private void Awake()
    {
        _col = GetComponent<Collider>();
    }
    private void Update()
    {
        //ShootUpBoxCast();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit");
        if (other.gameObject.CompareTag("Player"))
            Debug.Log("player connect");
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
            float time = hit.collider.GetComponent<StayTimeChecker>().StayTime;

            float weight = hit.collider.GetComponent<ObjWeight>().so.weight;

            float ratio = time * weight; //��Ƽ���� ũ�� ����cnt�� ũ�� �����䰡 ������ ���� cnt�� �۾�

            jumpPower = 1.5f * ratio;

            //�������� ���߸� �� 
            //Debug.Log(jumpPower);
            if (isJump == false)
            {
                if (jumpPower <= 0)
                {
                    return;
                }
                jumpPower--;
                isJump = true;
            }

             hit.collider.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
        else
        {
            isJump = false;
        }
    }
}
