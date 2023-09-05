using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerRenderer : MonoBehaviour
{
    private DissolveAnimator _dissolveAnimator = null;
    public DissolveAnimator dissolveAnimator => _dissolveAnimator;

    private bool _fliped = false;
    public bool Fliped => _fliped;
    private bool _lastFliped = false;

    [SerializeField]
    private UnityEvent<bool> OnFliped = null;

    private Player _player = null;

    public Vector3 Forward => transform.forward;
    public Vector3 Down => -transform.up;

    private Sequence _flipSeq = null;
    private DirectionType _flipDirection = DirectionType.Down;
    public DirectionType flipDirection => _flipDirection;
    [SerializeField]
    private float _flipTime = 1f;
    private bool _fliping = false;
    [SerializeField]
    private float _rotationSpeed = 20f;
    [SerializeField]
    private float _camShakePower = 5f;

    [SerializeField]
    private float _autoRotateDelay = 0.2f;
    [SerializeField]
    private float _autoRotateTime = 0.3f;
    private Sequence _autoRotateSeq = null;
    private float _autoRotateTimer = 0f;
    private bool _autoRotating = false;

    public bool Fliping => _fliping;

    private void Awake()
    {
        _player = GetComponentInParent<Player>();
        _dissolveAnimator = GetComponent<DissolveAnimator>();
    }

    private void Update()
    {
        _autoRotateTimer += Time.deltaTime;
        if(_autoRotateTimer > _autoRotateDelay)
        {
            AutoRotate();
        }
    }

    private void AutoRotate()
    {
        if (_fliping || _autoRotating)
            return;
        _autoRotating = true;

    }

    public bool GetHorizontalFlip()
    {
        return (_flipDirection == DirectionType.Up) || (_flipDirection == DirectionType.Down);
    }

    public void FlipSmoothSequence(DirectionType dirType)
    {
        _flipDirection = DirectionType.None;
        FlipDirectionChange(dirType, false);
    }

    public void FlipDirectionChange(DirectionType dirType, bool forceFlip)
    {
        if (_flipDirection == dirType)
            return;
        _flipDirection = dirType;
        Vector3 targetRotation = Vector3.zero;
        targetRotation = _flipDirection switch
        {
            DirectionType.None => Vector3.zero,
            DirectionType.Left => new Vector3(_player.transform.rotation.eulerAngles.y, 180f, 90f),
            DirectionType.Right => new Vector3(_player.transform.rotation.eulerAngles.y, 0f, 90f),
            DirectionType.Up => new Vector3(180f, _player.transform.rotation.eulerAngles.y, 0f),
            DirectionType.Down => new Vector3(0f, _player.transform.rotation.eulerAngles.y, 0f),
            _ => Vector3.zero,
        };

        if (_flipSeq != null)
            _flipSeq.Kill();
        if (forceFlip)
        {
            _player.transform.rotation = Quaternion.Euler(targetRotation);
        }
        else
        {
            //CamManager.Instance.GravityChangeCameraAnimation(null, dirType, _camRotatePower, 0.2f);
            CamManager.Instance.CameraShake(0.25f, _camShakePower, 3f);
            _flipSeq = DOTween.Sequence();
            _fliping = true;
            _flipSeq.Append(_player.transform.DORotate(targetRotation, _flipTime));
            _flipSeq.AppendCallback(() => { _fliping = false; });
        }
    }

    public Quaternion GetFlipedRotation(DirType dirType, RotAxis rotAxis)
    {
        //오른쪽 기준
        Quaternion rot = _player.transform.rotation; // left = -90
        switch (rotAxis)
        {
            case RotAxis.None:
                break;
            case RotAxis.X:
                rot = Quaternion.Euler(rot.eulerAngles.y, 0f, 0f);
                break;
            case RotAxis.Y:
                rot = Quaternion.Euler(0f, rot.eulerAngles.y, 0f);
                break;
            case RotAxis.Z:
                rot = Quaternion.Euler(0f, 0f, rot.eulerAngles.y);
                break;
            default:
                break;
        }

        if (dirType == DirType.Back)
            rot = Quaternion.Inverse(rot);

        return rot;
    }

    public Quaternion GetTargetRotation()
    {
        Quaternion targetRotation = Quaternion.identity;
        if (_flipDirection == DirectionType.Left)
        {
            targetRotation = Quaternion.Euler(_fliped ? -90f : 90f, 180f, 90f); //플레이어 로테이션을 돌린다 
        }
        else if (_flipDirection == DirectionType.Right)
        {
            targetRotation = Quaternion.Euler(_fliped ? -90f : 90f, 0f, 90f); //플레이어 로테이션을 돌린다 
        }
        else
        {
            targetRotation = Quaternion.Euler(_player.transform.rotation.eulerAngles.x, _fliped ? -90f : 90f, _player.transform.rotation.eulerAngles.z); //플레이어 로테이션을 돌린다 
        }
        return targetRotation;
    }

    public void Flip(Vector2 dir, bool smoothing = true)
    {
        if (dir.x == 0f || _fliping)
            return;

        DirectionType flipDir = DirectionType.None;
        if (dir.x > 0f)
            flipDir = DirectionType.Right;
        else if (dir.x < 0f)
            flipDir = DirectionType.Left;
        else
            flipDir = _fliped ? DirectionType.Left : DirectionType.Right;
        _fliped = flipDir == DirectionType.Left;

        Quaternion targetRotation = GetTargetRotation();

        if (smoothing)
            _player.transform.rotation = Quaternion.Slerp(_player.transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        else
            _player.transform.rotation = targetRotation;


        if (_lastFliped == _fliped)
            return;
        _lastFliped = _fliped;
        OnFliped?.Invoke(_fliped);
    }

    public void ForceFlip()
    {
        Quaternion targetRotation = Quaternion.Euler(_player.transform.rotation.eulerAngles.x, _fliped ? 90f : -90f, _player.transform.rotation.eulerAngles.z); // 반대
        _player.transform.rotation = targetRotation;
        _fliped = !_fliped;
        OnFliped?.Invoke(_fliped);
    }
}
