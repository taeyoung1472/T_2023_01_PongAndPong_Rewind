using UnityEngine;

public class GimmickDoor : ControlAbleObjcet
{
    [SerializeField] private Vector3 positiveMoveValue;
    [SerializeField] private Vector3 negativeMoveValue;
    [SerializeField] private float speed = 1;
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
