using UnityEngine;

public class PlayerGimmickObject : RigidbodyGimmickObject
{
    private Player _player = null;
    [SerializeField]
    private float _springAmplification = 5f;
    [SerializeField]
    private float _springTime = 1f;

    public override void AddForce(Vector3 dir, float force, ForceMode forceMode = ForceMode.Impulse)
    {
        //if (force <= _minSpringForce)
        //    force = _minSpringForce;

        _player.GetPlayerAction<PlayerJump>().ForceJump(dir, _springAmplification, _springTime);
    }

    public override void Init()
    {
        _player.ForceStop();
    }

    public override void RecordTopPosition()
    {
    }

    private void Start()
    {
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        RecordTopPosition();
    }
}
