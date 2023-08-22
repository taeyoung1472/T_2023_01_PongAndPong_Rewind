using UnityEngine;

public class GimmickDoor : ControlAbleObjcet
{
    [SerializeField] private Vector3 positiveMoveValue;
    [SerializeField] private Vector3 negativeMoveValue;
    [SerializeField] private float speed = 1;
    [SerializeField] private Transform arrow;
    private Vector3 originPos;

    public override void Control(ControlType controlType, bool isLever, Player player, DirectionType dirType)
    {
        if (curControlType != controlType)
        {
            if(controlType != ControlType.None)
            {
                TimeStampManager.Instance.SetStamp(StampType.doorOpen, controlColor);
            }
            else
            {
                //TimeStampManager.Instance.SetStamp(StampType.doorClose, controlColor);
            }
        }
        curControlType = controlType;
    }

    public override void SetColor()
    {
        if (arrow)
        {
            arrow.transform.Find("Arrow").GetComponent<SpriteRenderer>().color = controlColor;
        }
    }

    public override void ResetObject()
    {
        transform.localPosition = originPos;
        curControlType = ControlType.None;
    }

    private void Awake()
    {
        originPos = transform.localPosition;
        RewindManager.Instance.InitPlay += () => this.enabled = true;
        RewindManager.Instance.InitRewind += () => this.enabled = false;
    }

    private void OnValidate()
    {
        if (arrow)
        {
            arrow.transform.position = transform.position + new Vector3(0, 0, -2.25f);
            arrow.transform.parent = transform.parent;
            arrow.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(positiveMoveValue.y, positiveMoveValue.x) * Mathf.Rad2Deg);
            arrow.gameObject.name = name + "_Arrow";
        }
    }

    private void Update()
    {
        Vector3 targetPos = Vector3.zero;
        switch (curControlType)
        {
            case ControlType.Control:
                targetPos = originPos + positiveMoveValue;
                break;
            case ControlType.None:
                targetPos = originPos;
                break;
            case ControlType.ReberseControl:
                targetPos = originPos + negativeMoveValue;
                break;
        }

        if (Vector3.Distance(transform.localPosition, targetPos) > 0.25f)
        {
            Vector3 dir = (targetPos - transform.localPosition).normalized;
            transform.localPosition = transform.localPosition + dir * speed * Time.deltaTime;
        }
    }
}
