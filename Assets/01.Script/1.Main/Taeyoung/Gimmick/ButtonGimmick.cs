using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
public class ButtonGimmick : GimmickObject
{
    bool isActive = false;
    bool isActivePlayer = false;

    [SerializeField] private ControlData[] controlDataArr;
    [SerializeField] private GimmickVisualLink visualLinkPrefab;
    [SerializeField] private Color color = Color.white;

    #region ���
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

    public float timer = 0f;

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

        if (gravityButton)
        {
            gravityInverseGimmick = FindObjectOfType<GravityInverseGimmick>();
        }
    }
    public override void InitOnPlay()
    {
        base.InitOnPlay();
        toggleing = false;
        toggleTime = origntToggleTime;
        timer = 0;
        foreach (var control in controlDataArr)
        {
            control.target.Control(ControlType.None, false, player, gravitChangeDirState);
            CamManager.Instance.RemoveTargetGroup(control.target.transform);
        }
    }
    public override void InitOnRewind()
    {
        base.InitOnRewind();
        isStart = false;
    }
    public void CheckGravityTimeDir()
    {
        //���϶����� �ð��� ��
            timer += Time.deltaTime;
        if (isActivePlayer)
        {
            gravityInverseGimmick.dirChangeDic.Add(timer, gravitChangeDirState);
           // Debug.Log(timer); 
            //foreach (var pair in gravityInverseGimmick.dirChangeDic)
            //{
            //    Debug.Log(pair.Key + "Ű�͹��" +  pair.Value.ToString());
            //}
        }
    }
    public void Update()
    {
        if (isRewind)// && rewindButton == false)
        {
            return;
        }

        if (gravityButton)
        {
            CheckGravityTimeDir();
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
                        //Debug.Log("��ư" + gravitChangeDirState);
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
            //������¿��� ��ư�� ��� �ߴµ� Ÿ�̹��� �ݴ��
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