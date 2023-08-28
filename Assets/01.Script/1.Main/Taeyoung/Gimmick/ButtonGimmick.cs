using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGimmick : GimmickObject
{
    private bool isActive = false;
    private bool isActivePlayer = false;
    private bool curActive;

    [Header("[주요 필드]")]
    [SerializeField] private ControlData[] controlDataArr;
    [SerializeField] private ColorCODEX codex;

    #region 토글
    [Header("[토글]")]
    [SerializeField] private bool isToggle;
    [SerializeField] private float toggleTime;
    private bool toggleing;
    private float origntToggleTime;
    private float timer = 0f;
    private Slider toggleSlider = null;
    #endregion

    [Header("[중력 관련]")]
    [SerializeField] private bool gravityButton;
    public DirectionType gravitChangeDirState;
    private GravityInverseGimmick gravityInverseGimmick;

    [SerializeField] private DirectionType preDirType;

    public void Start()
    {
        toggleSlider = GetComponentInChildren<Slider>();
        SetSlider();
        if (RewindManager.Instance)
        {
            RewindManager.Instance.RestartPlay += () =>
            {

            };
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
        if (isRewind)
            return;

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
            Debug.Log(curActive);
            AudioManager.PlayAudio(SoundType.OnActiveButton);
            curActive = isFunc;
        }

        foreach (var control in controlDataArr)
        {
            if (isFunc)
                controlType = ControlType.Control;

            control.target.Control(controlType, control.isLever, player, gravitChangeDirState);
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