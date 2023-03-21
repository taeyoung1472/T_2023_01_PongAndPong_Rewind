using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PortalTelepote : MonoBehaviour
{
    public Transform playerTrm;
    public Transform reciverTrm;

    private Collider _col;
    private bool isPlayerOverlapping;
    public float rayDistance = 1f;
    private bool isHit;
    public bool isFront;
    public CinemachineVirtualCamera Vcam;

    public Transform renderTrm;

    public bool isRight = true;
    float dotProduct;
    private void Awake()
    {
        _col = GetComponent<Collider>();
    }
    private void Update()
    {

        if (isPlayerOverlapping)
        {
            playerTrm.GetComponent<CharacterController>().enabled = false;
            Vector3 portalToPlayer = playerTrm.position - transform.position;

            if (isRight)
            {
                dotProduct = Vector3.Dot(transform.right * 1f, portalToPlayer);
            }
            else
            {
                dotProduct = Vector3.Dot(transform.right * -1f, portalToPlayer);
            }


            Debug.Log(dotProduct);
            // If this is true: The player has moved across the portal
            if (dotProduct > 0f)
            {
                // Teleport him!
                float rotationDiff = -Quaternion.Angle(transform.rotation, reciverTrm.rotation);
                rotationDiff += 180;
                playerTrm.Rotate(Vector3.up, rotationDiff);

                Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
                Debug.Log(positionOffset);
                playerTrm.position = reciverTrm.position + positionOffset + Vector3.right * positionOffset.x;
                //playerTrm.position = new Vector3(playerTrm.position.x, playerTrm.position.y, -1.84f);
                isPlayerOverlapping = false;
            }
        }
        else
        {
            playerTrm.GetComponent<CharacterController>().enabled = true;
        }
    }
  
    private void FixedUpdate()
    {
        RaycastHit hit;

        Vector3 boxCenter = _col.bounds.center;
        Vector3 halfExtents = _col.bounds.extents;

        isHit = Physics.BoxCast(boxCenter, new Vector3(halfExtents.x, halfExtents.y, halfExtents.z), renderTrm.up, out hit, transform.rotation, rayDistance);

        if (isHit)
        {
            isFront = true;
        }
        else
        {
            isFront = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("잉이이");
        if (other.CompareTag("GimmickPlayerCol"))
        {
            Debug.Log("플레이어와 충돌함");
            isPlayerOverlapping = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GimmickPlayerCol"))
        {
            isPlayerOverlapping = false;
        }
    }
}
