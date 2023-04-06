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

    public void InteractStart(Player player)
    {
        if (_interactable == false)
            return;
        _player = player;
        OnInteractStart?.Invoke();
        ChildInteractStart();
    }

    public void InteractEnd(bool interactExit)
    {
        if(interactExit)
            _player.PlayerActionExit(PlayerActionType.Interact);
        OnInteractEnd?.Invoke();
        ChildInteractEnd();
    }

    protected abstract void ChildInteractEnd();

    protected abstract void ChildInteractStart();

    public virtual void InteractEnter()
    {

    }

    public virtual void InteractExit()
    {

    }
}
