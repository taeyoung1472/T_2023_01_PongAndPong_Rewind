using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using DG.Tweening;

public class CameraManager : MonoSingleTon<CameraManager>
{
    private float _originLens = 0f;

    private Coroutine _zoomCoroutine = null;
    private Coroutine _shakeCoroutine = null;

    private float _currentShakeAmount = 0f;

    [SerializeField]
    private CinemachineVirtualCamera _currentCam = null;
    [SerializeField]
    private CinemachineBasicMultiChannelPerlin _currentNoise = null;

    private List<CinemachineVirtualCamera> cams = new List<CinemachineVirtualCamera>();

    private CinemachineVirtualCamera _lastCam = null;
    private Sequence _sizeSeq = null;

    /// <summary>
    /// ī�޶� �ʱ�ȭ
    /// </summary>
    private void Awake()
    {
        cams.AddRange(GetComponentsInChildren<CinemachineVirtualCamera>());
        _originLens = FindObjectOfType<CinemachineVirtualCamera>().m_Lens.FieldOfView;
        CameraSelect(VCamTwo);
    }


    /// <summary>
    /// ī�޶� ���� �Լ�
    /// </summary>
    public void CameraSelect(CinemachineVirtualCamera cam)
    {
        CartCamReset();
        for(int i = 0; i < cams.Count; i++)
            if (cam == cams[i])
            {
                cams[i].gameObject.SetActive(true);
                _currentCam = cams[i];
                _currentNoise = cams[i].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            }
            else
                cams[i].gameObject.SetActive(false);
    }

    /// <summary>
    /// ���������� �� ķ ����
    /// </summary>
    public void LastCamSelect()
    {
        CartCamReset();
        if (_lastCam != null)
            CameraSelect(_lastCam);
    }

    /// <summary>
    /// Ʈ���� ���󰡴� ķ ����
    /// </summary>
    public void CartCamSelect(CinemachineSmoothPath path,  Transform look, float speed, LayerMask cullingMask = default(LayerMask))
    {
        _lastCam = _currentCam;
        CameraSelect(CartCam);
        if(cullingMask != default(LayerMask))
            Cam.cullingMask = cullingMask;
        Cam.orthographic = false;
        CartCam.LookAt = look;
        CinemachineDollyCart cart = CartCam.GetComponent<CinemachineDollyCart>();
        cart.m_Path = path;
        cart.m_Position = 0f;
        cart.m_Speed = speed;
        cart.enabled = true;
    }

    /// <summary>
    /// īƮķ ����
    /// </summary>
    public void CartCamReset()
    {
        Cam.orthographic = true;
        
        CartCam.Follow = null;
        CinemachineDollyCart cart = CartCam.GetComponent<CinemachineDollyCart>();
        cart.m_Path = null;
        cart.m_Position = 0f;
        cart.m_Speed = 0f;
        cart.enabled = false;
        Cam.cullingMask = -1;
    }

    /// <summary>
    /// Ʈ�� ����� ķ ���� ���� �� ����
    /// </summary>
    public void CartUpdate(float? orthoSize, float? position, float? speed)
    {
        CinemachineDollyCart cart = CartCam.GetComponent<CinemachineDollyCart>();
        if (_currentCam == CartCam)
        {
            if (orthoSize != null)
                OrthoDotw(orthoSize.Value);
            if (position != null)
                cart.m_Position = position.Value;
            if (speed != null)
                cart.m_Speed = speed.Value;
        }
    }

    /// <summary>
    /// ����׷��� �� ����
    /// </summary>
    private void OrthoDotw(float endVal)
    {
        if (_sizeSeq != null)
            _sizeSeq.Kill();
        _sizeSeq = DOTween.Sequence();
        _sizeSeq.Append(DOTween.To(() => CartCam.m_Lens.FieldOfView, x => CartCam.m_Lens.FieldOfView = x, endVal, 0.5f));
        //_sizeSeq.Append(DOTween.To(() => CartCam.m_Lens.OrthographicSize, x => CartCam.m_Lens.OrthographicSize = x, endVal, 0.5f));
    }

    public void CompletePrevFeedBack()
    {
        if (_shakeCoroutine != null)
        {
            StopCoroutine(_shakeCoroutine);
        }

        _currentNoise.m_FrequencyGain = 0; // ���� �� ����
        _currentNoise.m_AmplitudeGain = 0;
        _currentShakeAmount = 0f;
    }

    /// <summary>
    /// ī�޶� ������
    /// </summary>
    /// <param name="��鸮�� ����"></param>
    /// <param name="��鸮�� ��"></param>
    /// <param name="��鸱 �ð�"></param>
    public void CameraShake(float amplitude, float intensity, float duration, bool conti = false)
    {
        if (_currentShakeAmount > amplitude)
        {
            return;
        }
        CompletePrevFeedBack();

        _currentNoise.m_AmplitudeGain = amplitude; // ��鸮�� ����
        _currentNoise.m_FrequencyGain = intensity; // ���� �� ����

        _currentShakeAmount = _currentNoise.m_AmplitudeGain;

        _shakeCoroutine = StartCoroutine(ShakeCorutine(amplitude, duration, conti));
    }

    private IEnumerator ShakeCorutine(float amplitude, float duration, bool conti)
    {
        float time = duration;
        while (time >= 0)
        {
            if (conti)
                _currentNoise.m_AmplitudeGain = amplitude;
            else
                _currentNoise.m_AmplitudeGain = Mathf.Lerp(0, amplitude, time / duration);

            yield return null;
            time -= Time.deltaTime;
        }
        CompletePrevFeedBack();
    }

    public void ZoomCamera(float maxValue, float time)
    {
        CameraReset();

        _zoomCoroutine = StartCoroutine(ZoomCoroutine(maxValue, time));
    }

    private IEnumerator ZoomCoroutine(float maxValue, float duration)
    {
        float time = 0f;
        float nextLens = 0f;
        float currentLens = _currentCam.m_Lens.FieldOfView;

        while (time <= duration)
        {
            nextLens = Mathf.Lerp(currentLens, maxValue, time / duration);
            Debug.Log(time / duration);
            _currentCam.m_Lens.FieldOfView = nextLens;
            yield return null;
            time += Time.deltaTime;
        }
    }

    public void CameraReset()
    {
        if (_zoomCoroutine != null)
        {
            StopCoroutine(_zoomCoroutine);
        }
    }
}
