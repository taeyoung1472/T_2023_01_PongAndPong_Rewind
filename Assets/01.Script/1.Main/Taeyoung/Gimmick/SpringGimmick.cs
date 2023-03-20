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

    private void OnTriggerEnter(Collider other)
    {
        ColliderEnter(other);
    }

    private void ColliderEnter(Collider target)
    {
        if (target.transform.root.TryGetComponent<GimmickObject>(out GimmickObject obj))
        {
            float recordPosY = obj.RecordPosY - transform.position.y;
            recordPosY = Mathf.Clamp(recordPosY, 0, 17.5f);

            obj.Init();
            obj.AddForce(Vector3.up, recordPosY, ForceMode.VelocityChange);
        }
    }
}
