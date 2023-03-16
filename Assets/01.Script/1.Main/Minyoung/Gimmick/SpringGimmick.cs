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

    //박스캐스트를 쏴서 캐릭터에 부터ㅇ이쓴ㄴ 그 체공시간을 아는걸 같고와야하는거지
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

            float ratio = time * weight; //라티오가 크면 점프cnt가 크고 라이토가 작으면 점프 cnt가 작아

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
                float X = exitCol.Key.gameObject.transform.position.x; //물체위치 

                float boxX = halfExtents.x; // 박스 캐스트 길이 반절
                if (X > boxX + transform.position.x || X < transform.position.x - boxX)
                {
                    decelerationColDic.Remove(exitCol.Key);
                    Debug.Log("나감?");
                }
            }
        }

    
    }

}
