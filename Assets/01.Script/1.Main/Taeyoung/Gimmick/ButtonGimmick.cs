using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ButtonGimmick : GimmickObject
{
    bool isActive = false;
    bool isActivePlayer = false;
    bool curActive;

    [SerializeField] private ControlData[] controlDataArr;
    [SerializeField] private GimmickVisualLink visualLinkPrefab;
    [SerializeField] private Color color = Color.white;

    [SerializeField] private bool isVisuaLinkDisable;
    [SerializeField] private bool isCameraControlDisable;

    #region 토글
    [SerializeField] private bool isToggle;
    [SerializeField] private float toggleTime;
    [SerializeField] private float origntToggleTime;
    [SerializeField] private bool toggleing;
    #endregion

    private Animator animator;

    [SerializeField] private bool gravityButton;
    public DirectionType gravitChangeDirState;
    private GravityInverseGimmick gravityInverseGimmick;

    private float timer = 0f;
    [SerializeField] private DirectionType preDirType;

    #region 토글 슬라이더 관련
    private Slider toggleSlider = null;

    #endregion

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
        toggleSlider = GetComponentInChildren<Slider>();
        SetSlider();
        if (RewindManager.Instance)
        {
            RewindManager.Instance.RestartPlay += () =>
            {
                InitInfo();
            };
        }

        if (!isVisuaLinkDisable)
        {
            foreach (var data in controlDataArr)
            {
                GimmickVisualLink link = Instantiate(visualLinkPrefab);
                link.Link(transform, data.target.transform, color);
                data.target.controlColor = color;
            }
        }

        if (gravityButton)
        {
            gravityInverseGimmick = FindObjectOfType<GravityInverseGimmick>();
        }
    }
    private void SetSlider()
    {
        toggleSlider.maxValue = origntToggleTime;
        toggleSlider.value = toggleSlider.maxValue;

        if (isToggle == false)
        {
            toggleSlider.gameObject.SetActive(false);
        }
    }
    private void InitInfo()
    {
        Control(false);
        SetSlider();
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
            if (isActive == false)
            {
                Control();
                toggleTime -= Time.deltaTime;
                toggleSlider.value -= Time.deltaTime;
            }
            if (toggleTime <= 0.0f)
            {
                Control(false);
                toggleing = false;
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
            SetSlider();

            if (this.player == null)
            {
                this.player = player;
                Debug.Log(player);
            }
        }

        if (other.TryGetComponent<GimmickObject>(out GimmickObject gimmickObject))
        {
            Debug.Log(gimmickObject);

            SetSlider();

            isActive = true;
            toggleing = true;
            toggleTime = origntToggleTime;
        }
    }

    public void OnTriggerExit(Collider other)
    {
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
            Debug.Log(curActive);
            AudioManager.PlayAudio(SoundType.OnActiveButton);
            curActive = isFunc;
        }

        foreach (var control in controlDataArr)
        {
            if (!isCameraControlDisable)
            {
                if (controlType == ControlType.None)
                    CamManager.Instance.RemoveTargetGroup(control.target.transform);
                else
                    CamManager.Instance.AddTargetGroup(control.target.transform);
            }

            if (isFunc)
                controlType = control.isReverse ? ControlType.ReberseControl : ControlType.Control;

            control.target.Control(controlType, control.isLever, player, gravitChangeDirState);
        }

        if (controlType == ControlType.None)
            animator.SetBool("IsActive", false);
        else
            animator.SetBool("IsActive", true);
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