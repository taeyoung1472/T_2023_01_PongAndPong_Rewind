using UnityEngine;

public class PlayerGimmickObject : RigidbodyGimmickObject
{
    private Player _player = null;
    [SerializeField]
    private float _springAmplification = 5f;
    [SerializeField]
    private float _minSpringForce = 3f;

    public override void AddForce(Vector3 dir, float force, ForceMode forceMode = ForceMode.Impulse)
    {
        if (force <= _minSpringForce)
            force = _minSpringForce;

        Debug.Log("스프링 점프 힘   " + force);
        _player.GetPlayerAction<PlayerJump>().ForceJump(dir, force * _springAmplification);
    }

    public override void Init()
    {
        _player.ForceStop();
    }

    public override void RecordTopPosition()
    {
        if (_player.characterController.velocity.y >= -1f)
        {
            recordPosY = transform.position.y;
        }
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
