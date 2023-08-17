using UnityEngine;
using UnityEngine.Events;

public abstract class Interact : MonoBehaviour
{
    [SerializeField]
    protected Transform _interactUIPos = null;
    [SerializeField]
    protected Sprite _interactSprite = null;

    [SerializeField]
    private UnityEvent OnInteractStart = null;
    [SerializeField]
    private UnityEvent OnInteractEnd = null;

    protected bool _interactable = true;
    public bool Interactable { get => _interactable; set => _interactable = value; }

    [SerializeField]
    protected ChainInteract _chainInteract = null;

    protected Player _player = null;

    public bool isRewind = false;

    public virtual void Awake()
    {
        if (RewindManager.Instance)
        {
            RewindManager.Instance.InitRewind += InitOnRewind;
            RewindManager.Instance.InitPlay += InitOnPlay;
        }
    }
    public virtual void InitOnRewind()
    {
        isRewind = true;
    }
    public virtual void InitOnPlay()
    {
        isRewind = false;
    }
    public void InteractStart(Player player)
    {
        _player = player;
        OnInteractStart?.Invoke();
        ChildInteractStart();
    }

    public void InteractEnd(bool interactExit)
    {
        if (interactExit)
            _player?.PlayerActionExit(PlayerActionType.Interact);
        OnInteractEnd?.Invoke();
        ChildInteractEnd();
    }

    protected abstract void ChildInteractEnd();

    protected abstract void ChildInteractStart();

    protected virtual void ChildInteractEnter()
    {

    }

    protected virtual void ChildInteractExit()
    {

    }

    public void InteractEnter()
    {
        if (UIGetter.Instance)
        {
            if (_interactUIPos != null)
                UIGetter.Instance.GetInteractUI(_interactUIPos.position, _interactSprite, KeyManager.keys[InputType.Interact]);
            ChildInteractEnter();
        }
    }

    public void InteractExit()
    {
        if (UIGetter.Instance)
        {
            UIGetter.Instance.PushUIs();
            ChildInteractExit();
        }
    }
}
