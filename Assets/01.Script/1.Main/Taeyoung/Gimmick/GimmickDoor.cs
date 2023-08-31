using System.Collections.Generic;
using UnityEngine;

public class GimmickDoor : ControlAbleObjcet
{
    [Header("[이동 변수]")]
    [SerializeField] private Vector3 moveValue;
    [SerializeField] private float speed = 1;

    [Header("[정보 전달]")]
    [SerializeField] private Transform arrow;
    [SerializeField] private Transform doorCenter;
    [SerializeField] private List<Transform> arrowList = new();
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

    public void Awake()
    {
        originPos = transform.localPosition;
        RewindManager.Instance.InitPlay += () => this.enabled = true;
        RewindManager.Instance.InitRewind += () => this.enabled = false;
    }

    public void OnValidate()
    {
        if (arrow && doorCenter)
        {
            arrow.transform.position = doorCenter.position + new Vector3(0, 0, -2.25f);
            arrow.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(moveValue.y, moveValue.x) * Mathf.Rad2Deg);

            foreach (var arrow in arrowList)
            {
                arrow.localEulerAngles = new Vector3(arrow.localEulerAngles.x, arrow.localEulerAngles.y, Mathf.Atan2(moveValue.y, moveValue.x) * Mathf.Rad2Deg);
            }
        }
    }

    public void Update()
    {
        Vector3 targetPos = Vector3.zero;
        switch (curControlType)
        {
            case ControlType.Control:
                targetPos = originPos + moveValue;
                break;
            case ControlType.None:
                targetPos = originPos;
                break;
        }

        if (Mathf.Abs(Vector3.Distance(transform.localPosition, targetPos)) > 0.1f)
        {
            Vector3 dir = (targetPos - transform.localPosition).normalized;
            transform.localPosition = transform.localPosition + dir * speed * Time.deltaTime;
        }
        else
        {
            transform.localPosition = targetPos;
        }
    }
}
