using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cinemachine.Utility;
/// <summary> 자유 시점 카메라 </summary>
[DisallowMultipleComponent]
public class FreeLookCamera : MonoBehaviour
{
    #region Public Fields
    [Header("Options"), Range(1f, 20f)]
    public float _moveSpeed = 5f;
    [Range(1f, 10f)]
    public float _rotationSpeed = 5f;
    public bool _wheelAcceleration = true; // 마우스 휠로 이동속도 증가/감소
    public LayerMask freeviewLayerMask;

    [Space]
    public KeyCode _moveLeft = KeyCode.A;
    public KeyCode _moveRight = KeyCode.D;
    public KeyCode _moveUp = KeyCode.W;
    public KeyCode _moveDown = KeyCode.S;

    [Space]
    public KeyCode _run = KeyCode.LeftShift;
    public KeyCode _cursorLock = KeyCode.LeftAlt;

    [Header("States")]
    public bool _isActivated = false;     // 활성화 플래그
    public bool _isCursorVisible = true;
    public bool drag = false;

    [Space]
    public float offsetX = 10;
    public float offsetY = 10;
    public float offsetZ = 10;
    public float centerX = 0;
    public float centerY = 5;

    public Vector3 initPos;

    #endregion


    #region Private Fields

    private Vector3 _moveDir;
    private Vector3 _worldMoveDir;

    private Transform _rig;
    public Transform Rig { get => _rig; set => _rig = value; }
    private Camera cam;


    private Vector3 Origin;
    private Vector3 Difference;
    private Vector3 ResetCamera;

    public LayerMask gameviewLayerMask;

    #endregion


    #region Unity Events
    private void Awake()
    {
        ResetCamera = initPos;
        InitRig();
        InitTransform();
        cam = Camera.main;
    }

    private void Update()
    {
        if (!_isActivated) return; // 기능 비활성화 상태에서는 모든 기능 정지

        CursorLock();
        if (_isCursorVisible) return; // 커서 보이는 상태에서는 이동, 회전 X

        
        GetInputs();
        //Rotate();
        DragMouseMove();
        Move();
    }
    void LateUpdate()
    {
        _rig.position = new Vector3(
            Mathf.Clamp(_rig.position.x, centerX - offsetX * 0.5f, centerX + offsetX * 0.5f),
            Mathf.Clamp(_rig.position.y, centerY - offsetY * 0.5f, centerY + offsetY * 0.5f),
            Mathf.Clamp(_rig.position.z, -offsetZ, 10));
    }
    private void OnEnable()
    {
        _rig.position = ResetCamera;
    }

    #endregion


    #region Init Methods
    private void InitRig()
    {
        _rig = new GameObject("Free Look Camera Rig").transform;
        _rig.position = transform.position;
        _rig.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);

        _rig.SetSiblingIndex(transform.GetSiblingIndex());
    }

    private void InitTransform()
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0f);
        transform.SetParent(_rig);
    }

    public void InitPosCam(Transform camPos)
    {
        if(camPos != null)
        {
            ResetCamera = camPos.position;
            centerX = camPos.position.x;
        }
    }

    #endregion


    #region Private Methods
    private void GetInputs()
    {
        // 1. Movement
        _moveDir = new Vector3(0, 0, 0);


        float wheel = Input.GetAxis("Mouse ScrollWheel");
        Vector3 camDir = _rig.localRotation * Vector3.forward;
        _rig.position += camDir * wheel* _moveSpeed * 50f * Time.deltaTime;

        if (Input.GetKey(_moveRight)) _moveDir.x += 1f;
        if (Input.GetKey(_moveLeft)) _moveDir.x -= 1f;
        if (Input.GetKey(_moveUp)) _moveDir.y += 1f;
        if (Input.GetKey(_moveDown)) _moveDir.y -= 1f;

        if (Input.GetKey(_run))
        {
            _moveDir *= 2f;
        }

        // 2. Drag

        
    }

    private void CursorLock()
    {
        if (Input.GetKeyDown(_cursorLock))
        {
            _isCursorVisible = !_isCursorVisible;

            Cursor.lockState = !_isCursorVisible ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = _isCursorVisible;
        }
    }

    private void DragMouseMove()
    {
        if (Input.GetMouseButton(0))
        {
            Difference = (cam.ScreenToViewportPoint(Input.mousePosition)) - cam.transform.position;
            if (drag == false)
            {
                drag = true;
                Origin = cam.ScreenToViewportPoint(Input.mousePosition);
                
            }

        }
        else
        {
            drag = false;
        }

        if (drag)
        {
            _rig.position = Origin - Difference;
        }

        //if (Input.GetMouseButton(1))
        //    _rig.position = ResetCamera;
    }
    private void Move()
    {
        if (_moveDir == Vector3.zero) return;

        _worldMoveDir = transform.TransformDirection(_moveDir);
        _rig.Translate(_worldMoveDir * _moveSpeed * Time.deltaTime, Space.World);
    }

    #endregion


    #region Public Methods

    /// <summary> 전체 기능 활성화/비활성화 </summary>
    public void Activate(bool state)
    {
        if(cam == null)                 
        {
            cam = Camera.main;
            gameviewLayerMask = cam.cullingMask;
        }

        if (state)
        {
            cam.cullingMask = gameviewLayerMask | freeviewLayerMask;
        }
        else
        {
            cam.cullingMask = gameviewLayerMask;
        }
        _isActivated = state;
    }

    #endregion
}