using UnityEngine;

public class GimmickDoor : ControlAbleObjcet
{
    [SerializeField] private Vector3 positiveMoveValue;
    [SerializeField] private Vector3 negativeMoveValue;
    [SerializeField] private float speed = 1;
    private Vector3 originPos;

    public override void Control(ControlType controlType, bool isLever, Player player)
    {
        curControlType = controlType;
    }

    private void Awake()
    {
        originPos = transform.position;
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

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * speed) ;
    }
}
