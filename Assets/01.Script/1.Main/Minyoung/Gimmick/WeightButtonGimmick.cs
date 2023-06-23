using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightButtonGimmick : GimmickObject
{
    public Queue<Collider> queue = new Queue<Collider>();
    [SerializeField] private float weight;
    private BoxCollider _col;
    [SerializeField] private float distance = 0.1f;

    [SerializeField] private float pushableWeight = 20;

    [SerializeField] private ControlData[] controlDataArr;
    [SerializeField] private GimmickVisualLink visualLinkPrefab;
    [SerializeField] private Color color = Color.white;
    public DirectionType gravitChangeDirState;

    public enum ColiderState 
    {
        Box,
        Capsule,
        Sphere,
    }
    public ColiderState coliderState;

    [ContextMenu("Gen Color")]
    public void GenColor()
    {
        color = Random.ColorHSV();
    }
    void Start()
    {
        _col = GetComponentInChildren<BoxCollider>();

        if (RewindManager.Instance)
        {
            RewindManager.Instance.RestartPlay += () =>
            {
                foreach (var control in controlDataArr)
                {
                    control.target.isLocked = false;
                }
            };
        }
        foreach (var data in controlDataArr)
        {
            GimmickVisualLink link = Instantiate(visualLinkPrefab);
            link.Link(transform, data.target.transform, color);
        }
    }

    void Update()
    {
        if (isRewind)
        {
            return;
        }

        if (pushableWeight < weight)
        {
            foreach (var control in controlDataArr)
            {
                if (control.isLever)
                {
                    control.target.Control(control.isReverse ? ControlType.ReberseControl : ControlType.Control, true, player, gravitChangeDirState);
                }
                else
                {
                    control.target.Control(control.isReverse ? ControlType.ReberseControl : ControlType.Control, false, player, gravitChangeDirState);
                }
                CamManager.Instance.AddTargetGroup(control.target.transform);
            }
        }
        else
        {
            foreach (var control in controlDataArr)
            {
                control.target.Control(ControlType.None, false, player, gravitChangeDirState);
                CamManager.Instance.RemoveTargetGroup(control.target.transform);
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 halfExtents = _col.bounds.extents;
        Vector3 boxCenter = _col.bounds.center;
        RaycastHit[] hits = Physics.BoxCastAll(boxCenter, halfExtents, transform.up, Quaternion.identity, distance);
        if (hits.Length >= 1)
        {
            //Debug.Log("Sdhishasf");
            weight = 0;
            queue.Clear();
            foreach (RaycastHit hit in hits)
            {
                //Debug.Log(hit.collider.name);
                if (hit.transform.TryGetComponent<ObjWeight>(out ObjWeight objWeight))
                {
                    this.weight += objWeight.so.weight;
                    queue.Enqueue(hit.collider);
                }
            }
            //Debug.Log(queue.Count);
            while (queue.Count > 0)
            {
                Collider colPop = queue.Dequeue();
                if (colPop is BoxCollider)
                {
                    coliderState = ColiderState.Box;
                }
                else if (colPop is CapsuleCollider)
                {
                    coliderState = ColiderState.Capsule;
                }
                else if (colPop is SphereCollider)
                {
                    coliderState = ColiderState.Sphere;
                }

                RaycastHit[] boxHits = Physics.BoxCastAll(colPop.bounds.extents, colPop.bounds.center, transform.up, Quaternion.identity, distance);

                //switch (coliderState)
                //{
                //    case ColiderState.Box:
                //        if (boxHits.Length >= 1)
                //        {
                //            foreach (var hit in boxHits)
                //            {
                //                if (hit.transform.TryGetComponent<ObjWeight>(out ObjWeight objWeight))
                //                {
                //                    Debug.Log("이제ㅐ엽예{외사항그만찾아시발련아");
                //                    this.weight += objWeight.so.weight;
                //                    queue.Enqueue(hit.collider);
                //                }
                //            }
                //        }
                //        break;
                //    case ColiderState.Capsule:
                //        RaycastHit[] capsuleHits = Physics.BoxCastAll(colPop.bounds.extents, colPop.bounds.center, transform.up, Quaternion.identity, distance);
                //        break;
                //    case ColiderState.Sphere:
                //        RaycastHit[] sphereHits = Physics.BoxCastAll(colPop.bounds.extents, colPop.bounds.center, transform.up, Quaternion.identity, distance);
                //        break;
             
                // }
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

    public override void Init()
    {
        throw new System.NotImplementedException();
    }
}
