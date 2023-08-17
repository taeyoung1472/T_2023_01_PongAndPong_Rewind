using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour, IPlayerEnableResetable
{
    [SerializeField]
    private Slider _hpSlider = null;
    private Player _player = null;

    [SerializeField]
    private GameObject _dieParticlePrefab = null;

    [SerializeField]
    private UnityEvent OnDie = null;

    private int _curHP = 0;
    public int CurHP
    {
        get => _curHP;
        set
        {
            _curHP = Mathf.Clamp(value, 0, _player.playerHealthSO.maxHP);
            if (_hpSlider != null)
                _hpSlider.value = _curHP;
            if (_curHP == 0)
                Die();
        }
    }

    public bool Died => _curHP <= 0;

    public void AddDamage(int damage)
    {
        CurHP -= damage;
    }

    public void Die()
    {
        Debug.Log("»ç¸Á!!");
        _player.PlayerActionExit(_player.GetAllActionTypesArray());
        _player.PlayerActionLock(true, _player.GetAllActionTypesArray());
        _player.ForceStop();
        _player.PlayerInput.enabled = false;
        _player.GravityModule.UseGravity = false;
        OnDie?.Invoke();
    }

    public void DieStart()
    {
        if (StageManager.Instance == null)
            return;
        StageManager.Instance.InputLock = true;
        Instantiate(_dieParticlePrefab, _player.transform.position + _player.Col.center, _player.transform.rotation);
        _player.PlayerRenderer.dissolveAnimator.DissolveStart(0f, 0.3f, new Vector3(0f, 1f, 0f));
    }

    public void Restart()
    {
        if (StageManager.Instance == null)
            return;
        StageManager.Instance.InputLock = false;
        StageManager.Instance.OnReStartArea();
    }

    public void EnableReset()
    {
        if (_player == null)
            _player = GetComponent<Player>();
        _curHP = _player.playerHealthSO.maxHP;
        if (_hpSlider != null)
        {
            _hpSlider.minValue = 0;
            _hpSlider.maxValue = _player.playerHealthSO.maxHP;
        }
        _player.PlayerRenderer.dissolveAnimator.DissolveReset();
    }
}
