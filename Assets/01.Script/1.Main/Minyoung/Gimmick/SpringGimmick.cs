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
    //박스캐스트를 쏴서 캐릭터에 부터ㅇ이쓴ㄴ 그 체공시간을 아는걸 같고와야하는거지
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

            float ratio = time * weight; //라티오가 크면 점프cnt가 크고 라이토가 작으면 점프 cnt가 작아

            jumpPower = 1.5f * ratio;

            //서서히ㅣ 멈추면 더 
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
