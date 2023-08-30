using System;
using UnityEditor;
using UnityEngine;

public class ButtonGimmick : GimmickObject
{
    private bool isActive = false;
    private bool isActivePlayer = false;
    private bool curActive;

    [Header("[주요 필드]")]
    [SerializeField] private ControlData[] controlDataArr;
    [SerializeField] private ColorCODEX codex;
    private GimmickVisualLink[] visualLinks;

    [Header("[중력 관련]")]
    [SerializeField] private bool gravityButton;
    public DirectionType gravitChangeDirState;
    private GravityInverseGimmick gravityInverseGimmick;
    private float timer = 0f;

    [SerializeField] private DirectionType preDirType;

    public void Start()
    {
        visualLinks = GetComponentsInChildren<GimmickVisualLink>();
        foreach (var link in visualLinks)
        {
            link.color = ColorManager.GetColor(codex);
        }

        if (gravityButton)
        {
            gravityInverseGimmick = FindObjectOfType<GravityInverseGimmick>();
        }
    }

    private void InitInfo()
    {
        Control(false);
        foreach (var control in controlDataArr)
        {
            control.target.isLocked = false;
            preDirType = DirectionType.None;
        }
        timer = 0;
        isActive = false;
    }
    public override void InitOnPlay()
    {
        base.InitOnPlay();
        timer = 0;
        InitInfo();
        foreach (var control in controlDataArr)
        {
            control.target.Control(ControlType.None, false, player, gravitChangeDirState);
        }
    }

    public void CheckGravityTimeDir()
    {
        timer += Time.deltaTime;
        if (isActivePlayer && preDirType != gravitChangeDirState)
        {
            gravityInverseGimmick.dirChangeDic.Add(timer, gravitChangeDirState);
            preDirType = gravitChangeDirState;
        }
    }
    public void Update()
    {
        if (isRewind)
            return;

        if (gravityButton)
            CheckGravityTimeDir();

        if (isActive)
        {
            Control();
        }
        else
        {
            Control(false);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (isRewind)
            return;

        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            isActivePlayer = true;

            if (this.player == null)
            {
                this.player = player;
            }
        }

        if (other.TryGetComponent<GimmickObject>(out GimmickObject gimmickObject))
        {
            isActive = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (isRewind)
            return;

        if (other.TryGetComponent<GimmickObject>(out GimmickObject gimmickObject))
        {
            isActive = false;
        }
        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            isActivePlayer = false;
        }
    }

    public void Control(bool isFunc = true)
    {
        ControlType controlType = ControlType.Control;
        if (isFunc == false)
            controlType = ControlType.None;

        if (isFunc != curActive)
        {
            AudioManager.PlayAudio(SoundType.OnActiveButton);
            curActive = isFunc;
        }

        foreach (var control in controlDataArr)
        {
            if (isFunc)
                controlType = ControlType.Control;

            control.target.Control(controlType, control.isLever, player, gravitChangeDirState);
        }

        foreach (var link in visualLinks)
        {
            link.Active(controlType == ControlType.Control);
        }
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

    public override void Init()
    {
    }
}

[Serializable]
public class ControlData
{
    public ControlAbleObjcet target;
    public bool isLever = false;
}