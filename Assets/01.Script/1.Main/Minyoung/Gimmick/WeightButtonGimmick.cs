using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightButtonGimmick : MonoBehaviour
{
    public Queue<Collider> queue = new Queue<Collider>();
    [SerializeField] private float weight;
    private BoxCollider _col;
    [SerializeField] private float distance = 0.1f;
    void Start()
    {
        _col = GetComponentInChildren<BoxCollider>();
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 halfExtents = _col.bounds.extents;
        Vector3 boxCenter = _col.bounds.center;
        RaycastHit[] hits = Physics.BoxCastAll(boxCenter, halfExtents, transform.up, Quaternion.identity, distance);
        Debug.Log(hits.Length);
        Debug.Log(hits[0].collider.name);
        if (hits.Length <= 0)
        {
            return;
        }
        else
        {
            Debug.Log("Sdhishasf");
            weight = 0;
            foreach (RaycastHit hit in hits)
            {
                Debug.Log(hit.collider.name);
                if (hit.transform.TryGetComponent<ObjWeight>(out ObjWeight objWeight))
                {
                    this.weight += objWeight.so.weight;
                    Debug.Log("Sex");
                    queue.Enqueue(hit.collider);
                }
            }
            while(queue.Count >= 0)
            {
            }
        }
    }
    public void OnDrawGizmos()
    {
        RaycastHit hit;
        bool isHit = Physics.BoxCast(transform.position, transform.lossyScale / 2, transform.up, out hit, transform.rotation, distance);

        Gizmos.color = Color.red;
        if (isHit)
        {
            Gizmos.DrawRay(transform.position, transform.up * hit.distance);
            Gizmos.DrawWireCube(transform.position + transform.up * hit.distance, transform.lossyScale);
        }
        else
        {
            Gizmos.DrawRay(transform.position, transform.forward * distance);
        }
    }
}
