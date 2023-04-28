using UnityEngine;

public class SpringGimmick : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        ColliderEnter(collision.collider);
    }

    private void ColliderEnter(Collider target)
    {
        if (target.transform.TryGetComponent<RigidbodyGimmickObject>(out RigidbodyGimmickObject obj))
        {
            Debug.Log("°®°í¤Ã¿Í");
            float recordPosY = obj.RecordPosY - transform.position.y;
            recordPosY = Mathf.Clamp(recordPosY, 0, 17.5f);

            obj.Init();
            obj.AddForce(Vector3.up, recordPosY, ForceMode.VelocityChange);
        }
    }
}
