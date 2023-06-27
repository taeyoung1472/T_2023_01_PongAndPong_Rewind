using Cinemachine;
using DG.Tweening;
using System.Collections;
using UniRx;
using UnityEngine;

public class CamManager : MonoSingleTon<CamManager>
{
    CinemachineTargetGroup cinemachineTargetGroup;
    CinemachineTargetGroup CinemachineTargetGroup
    {
        get
        {
            if (cinemachineTargetGroup == null)
            {
                cinemachineTargetGroup = FindObjectOfType<CinemachineTargetGroup>();
            }
            return cinemachineTargetGroup;
        }
    }
    private CinemachineVirtualCamera _vCam = null;
    public CinemachineVirtualCamera VCam
    {
        get
        {
            if (_vCam == null)
            {
                _vCam = Utility.SearchByName<CinemachineVirtualCamera>("VCam");
            }
            return _vCam;
        }
    }

    private CinemachineBasicMultiChannelPerlin _vCamPerlin = null;

    private Sequence _vCamSeq = null;
    private Coroutine _shakeCoroutine = null;

    private void Start()
    {
        if(GameObject.Find("VCam") != null)
            _vCamPerlin = VCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void AddTargetGroup(Transform target, float weight = 1f, float radius = 3f)
    {
        if (CinemachineTargetGroup == null || target == null)
            return;

        if (CinemachineTargetGroup.FindMember(target) > -1)
            return;
        CinemachineTargetGroup.AddMember(target, weight, radius);

        //Debug.Log($"Add {target.name} {Time.time}");
    }

    public void RemoveTargetGroup(Transform target)
    {
        if (CinemachineTargetGroup == null || target == null)
            return;

        CinemachineTargetGroup.RemoveMember(target);
        //Debug.Log($"Remove {target.name} {Time.time}");
    }

    public void TargetGroupReset()
    {
        if (CinemachineTargetGroup == null)
            return;
        foreach (var tg in CinemachineTargetGroup.m_Targets)
            CinemachineTargetGroup.RemoveMember(tg.target);
    }

    public void GravityChangeCameraAnimation(Transform follow, DirectionType dirType, float power, float time)
    {
        Vector3 endEuler = dirType switch
        {
            DirectionType.None => Vector3.zero,
            DirectionType.Left => new Vector3(0, -1, 0),
            DirectionType.Right => new Vector3(0, 1, 0),
            DirectionType.Up => new Vector3(-1, 0, 0),
            DirectionType.Down => new Vector3(1, 0, 0),
            _ => Vector3.zero,
        };

        VCam.LookAt = null;
        VCam.Follow = follow;
        if (_vCamSeq != null)
            _vCamSeq.Kill();
        _vCamSeq = DOTween.Sequence();
        _vCamSeq.Append(VCam.transform.DORotate(endEuler * power + VCam.transform.rotation.eulerAngles, time)).SetLoops(2, LoopType.Yoyo)
            .OnComplete(() => { VCam.LookAt = cinemachineTargetGroup.transform; VCam.Follow = cinemachineTargetGroup.transform; });
    }

    public void CameraShake(float time, float emp, float fre)
    {
        if(_vCamPerlin == null)
        {
            Debug.LogWarning("_vCamPerlin ¾øÀ½.");
            return;
        }    
        if (_shakeCoroutine != null)
            StopCoroutine(_shakeCoroutine);
        _shakeCoroutine = StartCoroutine(CameraShakeCoroutine(time, emp, fre));
    }

    private IEnumerator CameraShakeCoroutine(float time, float emp, float fre)
    {
        float t = 1f;
        while (t >= 0f)
        {
            _vCamPerlin.m_AmplitudeGain = emp * t;
            _vCamPerlin.m_FrequencyGain = fre * t;

            t -= Time.deltaTime * (1 / time);
            yield return null;
        }
        _vCamPerlin.m_AmplitudeGain = 0f;
        _vCamPerlin.m_FrequencyGain = 0f;
    }
}
