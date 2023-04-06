using UnityEngine;
using DG.Tweening;

public class ElevatorInteract : Interact
{
    private bool _interacting = false;
    private Vector2 targetDir = Vector2.zero;

    [SerializeField]
    private Transform _playerPosition = null;
    public Transform PlayerPosition => _playerPosition;

    [SerializeField]
    private string _areaName = "";

    public string AreaName => _areaName;

    private Animator _animator = null;

    protected override void ChildInteractEnd()
    {
    }

    protected override void ChildInteractStart()
    {
        ElevatorManager.Instance.ElevatorInit(this);
    }

    public override void InteractEnter()
    {
        UIGetter.Instance.GetInteractUI(_interactUIPos.position, _interactSprite, KeyManager.keys[InputType.Interact]);
    }

    public override void InteractExit()
    {
        UIGetter.Instance.PushUIs();
    }

    private void Update()
    {
        if (_interacting == false)
            return;
        PlayerFlip();
    }

    private void PlayerFlip()
    {
        _player.PlayerRenderer.Flip(targetDir);
    }

    public void ElevatorAnimation()
    {
        targetDir = _playerPosition.position - _player.transform.position;
        targetDir.y = 0f;
        _interacting = true;
        _player.PlayerAnimation.MoveAnimation(targetDir);

        BoxCollider interactCol = transform.Find("InteractCollider").GetComponent<BoxCollider>();
        float interactColliderSize = interactCol.size.x;
        interactColliderSize *= 0.5f;
        float moveDuration = Mathf.Abs(transform.position.x + interactCol.center.x - _player.transform.position.x) / Mathf.Abs(transform.position.x + interactColliderSize * ((targetDir.x < 0f) ? -1f : 1f) - _player.transform.position.x);

        Sequence seq = DOTween.Sequence();
        seq.Append(_player.transform.DOMoveX(_playerPosition.position.x, moveDuration * 0.5f).SetEase(Ease.Linear));
        seq.AppendCallback(() =>
        {
            _interacting = false;
            _player.PlayerAnimation.MoveAnimation(Vector2.zero);
        });
        seq.Append(_player.transform.DORotate(Vector2.zero, 0.5f));
        seq.AppendCallback(() =>
        {
            _player.transform.rotation = Quaternion.identity;
            ElevatorManager.Instance.PlayCutScene();
        });
    }

    public void Animation(bool open)
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();
        if (open)
            _animator.Play("ElevatorOpen");
        else
            _animator.Play("ElevatorClose");
    }
}
