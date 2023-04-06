using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCon : MonoBehaviour
{
    public CharacterController characterController;
    void Update()
    {
        characterController.Move(10 * new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.GetComponent<Rigidbody>();

        Vector3 forceDir = hit.gameObject.transform.position - transform.position;
        rb.AddForceAtPosition(forceDir, transform.position, ForceMode.Impulse);
    }
}
