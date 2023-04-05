using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MeshTrailStruct
{
    public GameObject myObj;

    public MeshFilter BodyMeshFilter;

    public Mesh bodyMesh;
}

public class PlayerTrail : MonoBehaviour
{
    [SerializeField]
    private Transform _spawnTrm = null; // 어디에 생성띠?

    [SerializeField]
    private Material _trailMaterial = null;
    [SerializeField, ColorUsage(true, true)]
    private Color _startFresnelColor = Color.white;
    [SerializeField, ColorUsage(true, true)]
    private Color _startBaseColor = Color.white;
    [SerializeField, ColorUsage(true, true)]
    private Color _endFresnelColor = Color.white;
    [SerializeField, ColorUsage(true, true)]
    private Color _endBaseColor = Color.white;
    [SerializeField]
    private GameObject _meshTrailPrefab = null;
    [SerializeField]
    private SkinnedMeshRenderer _skinnedMeshRenderer = null; // ㅎㅎ

    private Queue<MeshTrailStruct> _readyTrails = new Queue<MeshTrailStruct>(); // 나올 수 있는 애들
    private Queue<MeshTrailStruct> _enalbeTrails = new Queue<MeshTrailStruct>(); // 꺼지고 있는 애들

    [SerializeField]
    private int _spawnCount = 0; // 처음에 몇 개?
    [SerializeField]
    private float _trailSpawnTime = 0.2f; // 생성 주기
    [SerializeField]
    private float _fadeDuration = 0.5f;
    private float _spawnTimer = 0f;

    [SerializeField]
    private bool _isMotionTrail = false;
    public bool IsMotionTrail { get => _isMotionTrail; set => _isMotionTrail = value; }

    private Transform _trailParentTrm = null;
    JustMono _mono = null;

    private bool _died = false;

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        Debug.Log("트레일 스크립트 부숴짐");
        DestroyTrailAll(true);
    }


    /// <summary>
    /// 그냥 다 부숴버림
    /// </summary>
    public void DestroyTrailAll(bool smooth)
    {
        Debug.Log("잔상 다 지움");
        _isMotionTrail = false;
        StopAllCoroutines();
        if (_mono == null)
            return;

        while (_enalbeTrails.Count > 0)
        {
            var trail = _enalbeTrails.Dequeue();
            if (smooth == false)
            {
                trail.myObj.SetActive(false);
            }
            else
            {
                MeshRenderer renderer = trail.BodyMeshFilter.GetComponent<MeshRenderer>();
                Action action = null;
                if (_enalbeTrails.Count == 0)
                {
                    action = () =>
                        {
                            _died = true;
                            for(int i = 0; i < _trailParentTrm.childCount; i++)
                            {
                                Destroy(_trailParentTrm.GetChild(i).gameObject);
                            }
                            Destroy(_trailParentTrm.gameObject);
                        };
                }
                _mono.
                StartCoroutine(FadeCoroutine(trail, renderer.materials[0].GetColor("_FresnelColor")
                , renderer.materials[0].GetColor("_BaseColor"),
                renderer.materials[0].GetFloat("_Alpha"), false, action));
            }

        }
    }

    private void Init()
    {
        Debug.Log("TrailInit");
        _trailParentTrm = new GameObject("TrailParentTrm").transform;
        _mono = _trailParentTrm.gameObject.AddComponent<JustMono>();
        for (int i = 0; i < _spawnCount; i++)
        {
            SpawnTrail();
        }
    }

    private void SpawnTrail()
    {
        MeshTrailStruct pss = new MeshTrailStruct();
        pss.myObj = Instantiate(_meshTrailPrefab, _trailParentTrm);
        pss.BodyMeshFilter = pss.myObj.transform.GetChild(0).GetComponent<MeshFilter>();

        pss.bodyMesh = new Mesh();
        _skinnedMeshRenderer.BakeMesh(pss.bodyMesh);
        pss.BodyMeshFilter.mesh = pss.bodyMesh;

        pss.myObj.SetActive(false);
        _readyTrails.Enqueue(pss);

        MeshRenderer renderer = pss.BodyMeshFilter.GetComponent<MeshRenderer>();
        List<Material> materials = new List<Material>();
        for (int i = 0; i < _skinnedMeshRenderer.materials.Length; i++)
            materials.Add(_trailMaterial);
        renderer.materials = materials.ToArray();
        materialUpdate(renderer, 1f, _startFresnelColor, _startBaseColor);
    }

    private void Update()
    {
        if (_isMotionTrail == false || _died)
            return;

        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= _trailSpawnTime) // 소환띠
        {
            _spawnTimer = 0f;
            PopTrail();
        }
    }

    private void PopTrail()
    {
        if (_readyTrails.Count == 0) // 새로 만들엉
        {
            SpawnTrail();
        }

        MeshTrailStruct trail = _readyTrails.Dequeue();
        trail.bodyMesh = new Mesh();
        _skinnedMeshRenderer.BakeMesh(trail.bodyMesh);
        trail.BodyMeshFilter.mesh = trail.bodyMesh;
        trail.myObj.SetActive(true);
        trail.myObj.transform.SetPositionAndRotation(_spawnTrm.position, _spawnTrm.rotation);
        StartCoroutine(FadeCoroutine(trail));
    }

    public void StartTrail()
    {
        _isMotionTrail = true;
        _spawnTimer = 0f;
    }

    public void EndTrail()
    {
        _isMotionTrail = false;
        _spawnTimer = 0f;
    }

    private IEnumerator FadeCoroutine(MeshTrailStruct trail)
    {
        _enalbeTrails.Enqueue(trail);
        float time = 1f;
        while (time >= 0f)
        {
            MeshRenderer renderer = trail.BodyMeshFilter.GetComponent<MeshRenderer>();
            Color fresnelColor = Color.Lerp(_endFresnelColor, _startFresnelColor, time);
            Color baseColor = Color.Lerp(_endBaseColor, _startBaseColor, time);
            materialUpdate(renderer, time, fresnelColor, baseColor);
            time -= Time.deltaTime * (1 / _fadeDuration);
            yield return null;
        }

        _enalbeTrails.Dequeue();
        trail.myObj.SetActive(false);
        _readyTrails.Enqueue(trail);
        yield break;
    }


    private IEnumerator FadeCoroutine(MeshTrailStruct trail, Color startFresnelColor, Color startBaseColor, float startAlpha, bool queueSetting, Action Callback = null)
    {
        if (queueSetting)
        {
            _enalbeTrails.Enqueue(trail);
        }
        float time = startAlpha;
        while (time >= 0f)
        {
            MeshRenderer renderer = trail.BodyMeshFilter.GetComponent<MeshRenderer>();
            Color fresnelColor = Color.Lerp(_endFresnelColor, startFresnelColor, time);
            Color baseColor = Color.Lerp(_endBaseColor, startBaseColor, time);
            materialUpdate(renderer, time, fresnelColor, baseColor);
            time -= Time.deltaTime * (1 / _fadeDuration);
            yield return null;
        }

        if (queueSetting)
        {
            _enalbeTrails.Dequeue();
            _readyTrails.Enqueue(trail);
        }
        trail.myObj.SetActive(false);
        Callback?.Invoke();
        yield break;
    }

    private void materialUpdate(MeshRenderer renderer, float alpha, Color fresnelColor, Color baselColor)
    {
        for (int i = 0; i < renderer.materials.Length; i++)
        {
            renderer.materials[i].SetFloat("_Alpha", alpha);
            renderer.materials[i].SetColor("_FresnelColor", fresnelColor);
            renderer.materials[i].SetColor("_BaseColor", baselColor);
        }
    }
}
