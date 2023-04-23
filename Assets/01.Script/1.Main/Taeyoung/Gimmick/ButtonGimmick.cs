using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
public class ButtonGimmick : GimmickObject
{
    bool isActive = false;

    [SerializeField] private ControlData[] controlDataArr;
    [SerializeField] private GimmickVisualLink visualLinkPrefab;
    [SerializeField] private Color color = Color.white;

    [SerializeField] private bool isToggle;
    [SerializeField] private float toggleTime;
    [SerializeField] private float origntToggleTime;
    [SerializeField] private bool toggleing;

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
        toggleTime = origntToggleTime;
    }
    public void Update()
    {
        if (isRewind)
        {
            return;
        }


        CheckPlayer();

        if (toggleing == true)
        {
            foreach (var control in controlDataArr)
            {
                if (control.isLever)
                {
                    control.target.Control(control.isReverse ? ControlType.ReberseControl : ControlType.Control, true, player);
                }
                else
                {
                    control.target.Control(control.isReverse ? ControlType.ReberseControl : ControlType.Control, false, player);
                }
            }
            toggleTime -= Time.deltaTime;
            if (toggleTime <= 0.0f)
            {
                isActive = false;
                foreach (var control in controlDataArr)
                {
                    control.target.Control(ControlType.None, false, player);
                }
            }
        }


        if (!isToggle)
        {
            if (isActive)
            {
                foreach (var control in controlDataArr)
                {
                    if (control.isLever)
                    {
                        control.target.Control(control.isReverse ? ControlType.ReberseControl : ControlType.Control, true, player);
                    }
                    else
                    {
                        control.target.Control(control.isReverse ? ControlType.ReberseControl : ControlType.Control, false, player);
                    }
                }
            }
            else
            {
                foreach (var control in controlDataArr)
                {
                    control.target.Control(ControlType.None, false, player);
                    CamManager.Instance.RemoveTargetGroup(control.target.transform);
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
            toggleing = true;

            if (this.player == null)
            {
                this.player = player;
                Debug.Log(player);
            }
        }
        if (other.TryGetComponent<GimmickObject>(out GimmickObject gimmickObject))
        {
            isActive = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<GimmickObject>(out GimmickObject gimmickObject))
        {
            isActive = false;
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