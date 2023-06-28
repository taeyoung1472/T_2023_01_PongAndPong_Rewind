using UnityEngine;

public class ScaleGimmick : GimmickObject
{
    private Collider leftCol;
    private Collider rightCol;
    [SerializeField] private float totalLength;

    private Vector3 leftOriginPos;
    private Vector3 rightOriginPos;

    public bool isCastOn = false;

    public LayerMask scaleLayerMask;
    public override void Init()
    {
        leftCol = transform.Find("Left").GetComponent<Collider>();
        rightCol = transform.Find("Right").GetComponent<Collider>();
    }

    public override void Awake()
    {
        base.Awake();
        Init();
    }
    private void Start()
    {
        leftOriginPos = leftCol.transform.localPosition;
        rightOriginPos = rightCol.transform.localPosition;

        leftCol.transform.localPosition = leftOriginPos + new Vector3(0, leftOriginPos.y - totalLength / 2, 0);
        rightCol.transform.localPosition = rightOriginPos + new Vector3(0, rightOriginPos.y - totalLength / 2, 0);
    }
    public void Update()
    {
        if (isRewind)
        {
            return;
        }
        Move();
    }
    public float left;
    public float right;

    private void Move()
    {
        float leftLength;
        float rightLength = 0;
        left = CalculLeftWeight();
        right = CalculRightWeight();

        if ((left == 0 && right == 0) || left == right)
        {
            rightLength = totalLength * 0.5f;
            leftLength = totalLength * 0.5f;
        }
        else if (left == 0)
        {
            rightLength = totalLength;
            leftLength = 0;
        }
        else if (right == 0)
        {
            rightLength = 0;
            leftLength = totalLength;
        }
        else
        {
            float total = right + left;
            float ratioRight = right * (1 / total);
            float ratioLeft = left * (1 / total);

            rightLength = totalLength * ratioRight;
            leftLength = totalLength * ratioLeft;
            Debug.Log(leftLength);
            Debug.Log(rightLength);
        }
        leftCol.transform.localPosition = Vector3.Lerp(leftCol.transform.localPosition, 
            leftOriginPos + new Vector3(0, leftOriginPos.y - leftLength, 0), Time.deltaTime);
        rightCol.transform.localPosition = Vector3.Lerp(rightCol.transform.localPosition, rightOriginPos + new Vector3(0, rightOriginPos.y - rightLength, 0), Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("충돌은 되는거임");
        if (collision.transform.TryGetComponent<ObjWeight>(out ObjWeight objWeight))
        {
           /// isCastOn = true;
            Debug.Log("무게체크시작");
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.TryGetComponent<ObjWeight>(out ObjWeight objWeight))
        {
            isCastOn = false;
            Debug.Log("무게체크멈춰");
        }
    }

    
    private float CalculRightWeight()
    {
        Vector3 boxCenter = rightCol.bounds.center + Vector3.up * rightCol.bounds.size.y;
        Vector3 halfExtents = rightCol.bounds.extents;
        Collider[] hitColliders = Physics.OverlapBox(boxCenter, halfExtents, Quaternion.identity, scaleLayerMask);
        float rightWeight = 0;
        foreach (var col in hitColliders)
        {
            //Debug.Log(col);

            if (col.transform == null)
                continue;

            ObjWeight obj = col.transform.GetComponent<ObjWeight>();
            if (obj == null)
                continue;
            rightWeight += obj.so.weight;
        }

        return rightWeight;
    }

    private float CalculLeftWeight()
    {
        Vector3 boxCenter = leftCol.bounds.center + Vector3.up * rightCol.bounds.size.y;
        Vector3 halfExtents = leftCol.bounds.extents;
        Collider[] hitColliders = Physics.OverlapBox(boxCenter, halfExtents, Quaternion.identity, scaleLayerMask);
        float leftWeight = 0;
        foreach (var col in hitColliders)
        {
            if (col.transform == null)
                continue;

            ObjWeight obj = col.transform.GetComponent<ObjWeight>();
            if (obj == null)
                continue;
            leftWeight += obj.so.weight;
        }

        return leftWeight;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Vector3 boxCenter = rightCol.bounds.center + Vector3.up * rightCol.bounds.size.y;
    //    Vector3 halfExtents = rightCol.bounds.extents;
    //    Gizmos.DrawWireCube(boxCenter, halfExtents);

    //    boxCenter = leftCol.bounds.center + Vector3.up * rightCol.bounds.size.y;
    //    halfExtents = leftCol.bounds.size;
    //    Gizmos.DrawWireCube(boxCenter, halfExtents);
    //}

}