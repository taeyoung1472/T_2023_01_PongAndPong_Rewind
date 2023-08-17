using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveAnimator : MonoBehaviour
{
    [SerializeField]
    private List<MeshRenderer> _targetRenderers = new List<MeshRenderer>();
    [SerializeField]
    private List<SkinnedMeshRenderer> _targetSkinRenderers = new List<SkinnedMeshRenderer>();

    [SerializeField]
    private float _dissolveTime = 0.8f;
    private List<Material> _materials = new List<Material>();
    private Sequence _dissolSeq = null;

    [Header("�׽�Ʈ��")]
    public float startVal = 0f;
    public float endVal = 1f;
    public float dissolveTime = 0.8f;
    public Vector3 dir = Vector3.up;

    private void Awake()
    {
        if (_targetRenderers != null)
        {
            for(int i = 0; i < _targetRenderers.Count; i++)
            {
                _materials.AddRange(_targetRenderers[i].materials);
            }
        }
        if (_targetSkinRenderers != null)
        {
            for (int i = 0; i < _targetSkinRenderers.Count; i++)
            {
                _materials.AddRange(_targetSkinRenderers[i].materials);
            }
        }
    }

    [ContextMenu("�׽�Ʈ ������")]
    public void TestDissolve()
    {
        DissolveStart(startVal, endVal, dir, dissolveTime);
    }

    public float GetDissolveRatio()
    {
        Material targetMat = _materials[0];
        if (targetMat == null)
            return 0f;
        return targetMat.GetFloat("_Dissolve");
    }

    /// <summary>
    /// ��ũ��Ʈ�� _dissolveTime �ð���ŭ ������˴ϴ�.
    /// </summary>
    /// <param name="dissolveDirection"></param>
    public void DissolveStart(float startValue, float endValue, Vector3 dissolveDirection)
    {
        DissolveStart(startValue, endValue, dissolveDirection, _dissolveTime);
    }

    /// <summary>
    /// dissolveDirection ������������ dissolveTime��ŭ ������˴ϴ�.
    /// </summary>
    /// <param name="dissolveDirection"></param>
    /// <param name="dissolveTime"></param>
    public void DissolveStart(float startValue, float endValue, Vector3 dissolveDirection, float dissolveTime)
    {
        if (_dissolSeq != null)
            _dissolSeq.Kill();
        _dissolSeq = DOTween.Sequence();
        foreach (var mat in _materials)
        {
            mat.SetFloat("_Dissolve", startValue);
            mat.SetVector("_DissolveDirection", dissolveDirection);
            _dissolSeq.Join(DOTween.To(() => 0f, x => mat.SetFloat("_Dissolve", x), endValue, dissolveTime)).SetUpdate(true);
        }
    }

    /// <summary>
    /// �����긦 0���� �ʱ�ȭ��ŵ�ϴ�.
    /// </summary>
    public void DissolveReset()
    {
        DissolveSet(0f);
    }

    /// <summary>
    /// �����긦 value�� �����մϴ�.
    /// </summary>
    /// <param name="value"></param>
    public void DissolveSet(float value)
    {
        if (_dissolSeq != null)
            _dissolSeq.Kill();
        foreach (var mat in _materials)
        {
            mat.SetFloat("_Dissolve", value);
        }
    }
}
