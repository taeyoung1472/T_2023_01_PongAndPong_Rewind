using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ButtonGimmick : GimmickObject
{
    bool isActive = false;
    bool isActivePlayer = false;

    [SerializeField] private bool onlyObj;

    [SerializeField] private ControlData[] controlDataArr;
    [SerializeField] private GimmickVisualLink visualLinkPrefab;
    [SerializeField] private Color color = Color.white;

    [SerializeField] private bool isToggle;
    [SerializeField] private float toggleTime;
    [SerializeField] private float origntToggleTime;
    [SerializeField] private bool toggleing;

    private Animator animator;

    public DirectionType gravitChangeDirState;

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
                foreach (var control in controlDataArr)
                {
                    control.target.isLocked = false;
                }
                isActive = false;
            };
        }
        foreach (var data in controlDataArr)
        {
            GimmickVisualLink link = Instantiate(visualLinkPrefab);
            link.Link(transform, data.target.transform, color);
        }
    }
    public override void InitOnPlay()
    {
        base.InitOnPlay();
        toggleing = false;
        toggleTime = origntToggleTime;
        foreach (var control in controlDataArr)
        {
            control.target.Control(ControlType.None, false, player, gravitChangeDirState);
            CamManager.Instance.RemoveTargetGroup(control.target.transform);
        }
    }
    public void Update()
    {
        if (isRewind)
        {
            return;
        }

        CheckPlayer();

        if (!isToggle)
        {
            if (isActive)
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
        if (toggleing == true && isToggle == true)
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
            }
            if (isActive == false)
            {
                toggleTime -= Time.deltaTime;
            }
            if (toggleTime <= 0.0f)
            {
                animator.Play("Idle");
                foreach (var control in controlDataArr)
                {
                    control.target.Control(ControlType.None, false, player, gravitChangeDirState);
                }
            }
        }


    }

    private void CheckPlayer()
    {
        if (isActive)
            return;

        foreach (var col in Physics.OverlapBox(transform.position, Vector3.one * 2.5f))
        {
            if (col.gameObject.TryGetComponent<Player>(out Player player))
            {
                foreach (var control in controlDataArr)
                {
                    CamManager.Instance.AddTargetGroup(control.target.transform);
                }

                return;
            }
        }

        foreach (var control in controlDataArr)
        {
            CamManager.Instance.RemoveTargetGroup(control.target.transform);
        }
        return;
    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            isActivePlayer = true;
            toggleing = true;
            toggleTime = origntToggleTime;

            animator.Play("Push");

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