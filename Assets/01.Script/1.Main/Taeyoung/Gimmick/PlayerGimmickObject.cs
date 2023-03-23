using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGimmickObject : GimmickObject
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

        Debug.Log("������ ���� ��   " + force);
        _player.GetPlayerAction<PlayerJump>().ForceJump(dir, force * _springAmplification);
    }

    public override void Init()
    {
        _player.ForceStop();
    }

    public override bool IsGimmickable(GameObject gimmickObj)
    {
        if (_player.transform.position.y < gimmickObj.transform.position.y) // �÷��̾ ���� ������ �ؿ� �ִٸ�
            return false;
        return true;
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
