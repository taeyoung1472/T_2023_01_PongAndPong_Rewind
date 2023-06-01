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
    private SkinnedMeshRenderer _meshRenderer = null;
    [SerializeField]
    private float _dissolveTime = 0.5f;
    private List<Material> _materials = new List<Material>();
    private Sequence _dissolSeq = null;

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

    private void Start()
    {
        if(_meshRenderer != null)
            _materials.AddRange(_meshRenderer.materials);
    }

    public void AddDamage(int damage)
    {
        CurHP -= damage;
    }

    private void Update()
    {
        //µπˆ±◊øÎ ƒ⁄µÂ
        if (Input.GetKeyDown(KeyCode.Y))
            Die();
    }

    public void Die()
    {
        Debug.Log("ªÁ∏¡!!");
        _player.ForceStop();
        _player.PlayerActionExit(_player.GetAllActionTypesArray());
        _player.PlayerActionLock(true, _player.GetAllActionTypesArray());
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
        if (_dissolSeq != null)
            _dissolSeq.Kill();
        _dissolSeq = DOTween.Sequence();
        foreach (var mat in _materials)
        {
            _dissolSeq.Join(DOTween.To(() => 0f, x => mat.SetFloat("_Dissolve", x), 0.3f, _dissolveTime)).SetUpdate(true);
        }
    }

    public void Restart()
    {
        if (StageManager.Instance == null)
            return;
        StageManager.Instance.InputLock = false;
        StageManager.Instance.OnReStartArea();
        _player.PlayerActionLock(false, _player.GetAllActionTypesArray());
        _player.PlayerInput.enabled = true;
        _player.GravityModule.UseGravity = true;
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
        if (_dissolSeq != null)
            _dissolSeq.Kill();
        foreach (var mat in _materials)
        {
            mat.SetFloat("_Dissolve", 0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        return;
        // ∏  ≈ª√‚, ∞Ê∞Ë π€
        if(other.CompareTag("Die"))
        {
            Die();
        }
    }
}
