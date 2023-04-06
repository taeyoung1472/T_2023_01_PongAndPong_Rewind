using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private Slider _hpSlider = null;
    private Player _player = null;

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

    private void Start()
    {
        _player = GetComponent<Player>();
        _curHP = _player.playerHealthSO.maxHP;
        if (_hpSlider != null)
        {
            _hpSlider.minValue = 0;
            _hpSlider.maxValue = _player.playerHealthSO.maxHP;
        }
    }

    public void AddDamage(int damage)
    {
        CurHP -= damage;
    }

    public void Die()
    {
        Debug.Log("»ç¸Á!!");
        _player.ForceStop();
        _player.PlayerInput.enabled = false;
        OnDie?.Invoke();
    }
}
