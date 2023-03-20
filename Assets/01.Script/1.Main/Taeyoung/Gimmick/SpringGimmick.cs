using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpringGimmick : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.TryGetComponent<GimmickObject>(out GimmickObject obj))
        {
            float recordPosY = obj.RecordPosY - transform.position.y;
            recordPosY = Mathf.Clamp(recordPosY, 0, 17.5f);

            obj.Init();
            obj.AddForce(Vector3.up, recordPosY, ForceMode.VelocityChange);
        }
    }
}
