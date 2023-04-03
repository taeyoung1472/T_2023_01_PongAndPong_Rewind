using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpringGimmick : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        ColliderEnter(collision.collider);
    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        ColliderEnter(hit.collider);
        Debug.Log("¤©¤©¤©¤©");
    }

    private void ColliderEnter(Collider target)
    {
        if (target.transform.root.TryGetComponent<RigidbodyGimmickObject>(out RigidbodyGimmickObject obj))
        {
            float recordPosY = obj.RecordPosY - transform.position.y;
            recordPosY = Mathf.Clamp(recordPosY, 0, 17.5f);

            obj.Init();
            obj.AddForce(Vector3.up, recordPosY, ForceMode.VelocityChange);
            Debug.Log("¤·¤·¤·¤·");
        }
    }
}
