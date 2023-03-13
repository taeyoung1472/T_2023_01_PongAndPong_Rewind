using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowGimmick : MonoBehaviour
{
    private Collider _col = null;

    [SerializeField] private float rayDistance = 1f;

    [SerializeField] private float slowSpeed = 0.5f;

    public bool isSlow = false;
    PlayerMove playerMove =  null;

    private void Awake()
    {
        _col = GetComponent<Collider>();
        //playerMove = FindObjectOfType<PlayerMove>();
    }
    private void FixedUpdate()
    {
        CheckObj();
    }
    public void CheckObj()
    {
        RaycastHit hit;
        Vector3 boxCenter = _col.bounds.center;
        Vector3 halfExtents = _col.bounds.extents;

         isSlow = Physics.BoxCast(boxCenter, new Vector3( halfExtents.x / 2f, halfExtents.y, halfExtents.z /2f), transform.up, out hit, transform.rotation, rayDistance);

        if (isSlow)
        {
            Debug.Log(hit.transform.name);
            playerMove = hit.transform.GetComponent<PlayerMove>();
            playerMove.isSlow = true; 

            //Gizmos.DrawRay(transform.position, transform.up * hit.distance);
            Debug.Log("슬로우");
        }
        else
        {
            if (playerMove != null)
            {
                playerMove.isSlow = false;
                playerMove = null;
            }
            Debug.Log("충돌 x"); 
        } 
    }
    
}
