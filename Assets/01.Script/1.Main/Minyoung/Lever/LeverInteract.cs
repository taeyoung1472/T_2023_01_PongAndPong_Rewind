using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class LeverInteract : GimmickObject
{
    [Header("[주요 필드]")]
    [SerializeField] private ControlData[] controlDataArr;
    [SerializeField] private ColorCODEX codex;
    private Transform handle;
    private LayerMask playerLayer;

    [Header("[중력 관련]")]
    private bool isPush = false;
    public DirectionType gravityChangeDir;

    private void Start()
    {
        playerLayer = 1 << LayerMask.NameToLayer("Player");
        handle = transform.Find("Handle");
    }

    public override void InitOnPlay()
    {
        base.InitOnPlay();
        isPush = false;

        foreach (var control in controlDataArr)
        {
            control.target.ResetObject();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Collider[] cols = Physics.OverlapBox(transform.position + new Vector3(0, 0.5f, 0), new Vector3(1, 0.5f, 1), Quaternion.identity, playerLayer);
            Debug.Log(cols.Length);
            if (cols.Length > 0)
            {
                LeverPullAction();
                return;
            }
        }
    }

    public void LeverPullAction()
    {
        isPush = !isPush;
        foreach (var control in controlDataArr)
        {
            if (isPush)
            {
                control.target.Control(ControlType.Control, true, player, gravityChangeDir);
            }
            else
            {
                control.target.Control(ControlType.None, true, player, gravityChangeDir);
            }
        }

        if (isPush)
        {
            handle.DOLocalRotate(new Vector3(130, -90, 0), 0.6f);
        }
        else
        {
            handle.DOLocalRotate(new Vector3(50, -90, 0), 0.6f);
        }
    }

    public override void Init()
    {

    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        foreach (var control in controlDataArr)
        {
            if (control.target == null)
                continue;

            Handles.color = Color.blue;
            Handles.DrawLine(transform.position, control.target.transform.position, 10);
        }
    }
#endif
}
