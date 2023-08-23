using UnityEngine;

public class LeverInteract : Interact
{
    [SerializeField] private ControlData[] controlDataArr;
    [SerializeField] private GameObject popup;
    [SerializeField] private ColorCODEX codex;
    [SerializeField] private GimmickVisualLink visualLinkPrefab;
    private Animator _animator = null;

    public bool isPush = false;

    public DirectionType gravityChangeDir;

    public void Start()
    {
        if (visualLinkPrefab != null)
        {
            foreach (var data in controlDataArr)
            {
                GimmickVisualLink link = Instantiate(visualLinkPrefab, transform);
                link.Link(transform, data.target.transform, ColorManager.GetColor(codex));
                data.target.controlColor = ColorManager.GetColor(codex);
                data.target.SetColor();
            }
        }
    }

    public override void InitOnPlay()
    {
        base.InitOnPlay();
        isPush = false;
        if (_animator == null)
            _animator = GetComponent<Animator>();
        _animator.Play("Idle");

        foreach (var control in controlDataArr)
        {
            control.target.ResetObject();
        }
        //popup.gameObject.SetActive(false);
    }

    protected override void ChildInteractEnd()
    {
    }

    //private void Update()
    //{
    //    if (popup != null && _player != null)
    //    {
    //        popup.gameObject.SetActive(Vector3.Distance(transform.position, _player.transform.position) <= 1.5f);
    //    }
    //}

    protected override void ChildInteractStart()
    {
        _player.PlayerInput.enabled = true;

        if (Vector3.Distance(transform.position, _player.transform.position) >= 1.5f)
            return;


        foreach (var control in controlDataArr)
        {
            if (control.target.isLocked)
            {
                return;
            }
        }

        LeverPullAction();
        LeverAnimation(isPush);
    }
    public void LeverPullAction()
    {
        isPush = !isPush; //처음에 ispush트루
        foreach (var control in controlDataArr)
        {
            if (isPush)
            {
                control.target.Control(control.isReverse ? ControlType.ReberseControl : ControlType.Control, true, _player, gravityChangeDir);
            }
            else
            {
                control.target.Control(ControlType.None, true, _player, gravityChangeDir);
            }
        }
    }

    public void LeverAnimation(bool pull)
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();



        if (pull)
        {
            _animator.Play("LeverPull");
        }
        else
        {
            _animator.Play("LeverPush");
        }
    }

}
