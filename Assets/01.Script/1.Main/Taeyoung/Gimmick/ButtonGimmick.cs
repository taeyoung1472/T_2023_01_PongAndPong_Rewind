using System;
using UnityEditor;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using Random = UnityEngine.Random;

public class ButtonGimmick : GimmickObject
{
    bool isActive = false;
    bool isActivePlayer = false;

    [SerializeField] private ControlData[] controlDataArr;
    [SerializeField] private GimmickVisualLink visualLinkPrefab;
    [SerializeField] private Color color = Color.white;

    #region 토글
    [SerializeField] private bool isToggle;
    [SerializeField] private float toggleTime;
    [SerializeField] private float origntToggleTime;
    [SerializeField] private bool toggleing;
    #endregion

    private bool isStart = false;


    private Animator animator;

    [SerializeField] private bool gravityButton;
    public DirectionType gravitChangeDirState;
    private GravityInverseGimmick gravityInverseGimmick;

    private float timer = 0f;
    [SerializeField] private DirectionType preDirType;

    [ContextMenu("Gen Color")]
    public void GenColor()
    {
        color = Random.ColorHSV();
    }

    public void Reset()
    {
        GenColor();
    }

    public void Start()
    {
        animator = GetComponent<Animator>();
        if (RewindManager.Instance)
        {
            RewindManager.Instance.RestartPlay += () =>
            {
                InitInfo();
            };
        }
        foreach (var data in controlDataArr)
        {
            GimmickVisualLink link = Instantiate(visualLinkPrefab);
            link.Link(transform, data.target.transform, color);
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
        toggleing = false;
        toggleTime = origntToggleTime;
        timer = 0;
        InitInfo();
        foreach (var control in controlDataArr)
        {
            control.target.Control(ControlType.None, false, player, gravitChangeDirState);
            CamManager.Instance.RemoveTargetGroup(control.target.transform);
        }
    }
 
    public void CheckGravityTimeDir()
    {
        timer += Time.deltaTime;
        if (isActivePlayer && preDirType != gravitChangeDirState)
        {
            gravityInverseGimmick.dirChangeDic.Add(timer, gravitChangeDirState);
            Debug.Log("딕셔너리에 추가됨");
            preDirType = gravitChangeDirState;
            
        }
    }
    public void Update()
    {
        if (isRewind)
            return;
        if (gravityButton)
            CheckGravityTimeDir();

        if (!isToggle)
        {
            if (isActive)
            {
                Control();
            }
            else
            {
                Control(false);
            }
        }
        if (toggleing == true && isToggle == true)
        {
            Control();
            if (isActive == false)
            {
                toggleTime -= Time.deltaTime;
            }
            if (toggleTime <= 0.0f)
            {
                animator.Play("Idle");
                Control(false);
            }
        }
    }
    
    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            isActivePlayer = true;
            toggleing = true;
            toggleTime = origntToggleTime;
            //역행상태에서 버튼을 밟게 했는데 타이밍이 반대야
            animator.Play("Push");
            //gravityInverseGimmick.dirChangeDic.Add(2, gravitChangeDirState);
            if (this.player == null)
            {
                this.player = player;
                Debug.Log(player);
            }
        }

        if (other.TryGetComponent<GimmickObject>(out GimmickObject gimmickObject))
        {
            Debug.Log(gimmickObject);
            isActive = true;
            toggleing = true;
            toggleTime = origntToggleTime;
            animator.Play("Push");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<GimmickObject>(out GimmickObject gimmickObject))
        {
            isActive = false;
            animator.Play("Idle");
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

        foreach (var control in controlDataArr)
        {
            if(controlType == ControlType.None)
                CamManager.Instance.RemoveTargetGroup(control.target.transform);
            else
                CamManager.Instance.AddTargetGroup(control.target.transform);

            if(isFunc)
                controlType = control.isReverse? ControlType.ReberseControl: ControlType.Control;

            control.target.Control(controlType, control.isLever, player, gravitChangeDirState);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        foreach (var control in controlDataArr)
        {
            if (control == null)
                continue;

            Handles.color = control.isReverse ? Color.red : Color.blue;
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
    public bool isReverse = true;
    public bool isLever = false;
    public bool isLocked = false;
}