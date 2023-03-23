using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrail : MonoBehaviour
{
    [SerializeField]
    private Transform _spawnTrm = null; // ��� ������?

    [SerializeField]
    private Material _trailMaterial = null;
    [SerializeField]
    private Color _startColor = Color.white;
    [SerializeField]
    private Color _endColor = Color.white;
    [SerializeField]
    private GameObject _meshTrailPrefab = null;
    [SerializeField]
    private Transform _trailParentTrm = null; // Ʈ���� �θ�
    [SerializeField]
    private SkinnedMeshRenderer _skinnedMeshRenderer = null; // ����

    private Queue<MeshTrailStruct> _readyTrails = new Queue<MeshTrailStruct>(); // ���� �� �ִ� �ֵ�
    private Queue<MeshTrailStruct> _enalbeTrails = new Queue<MeshTrailStruct>(); // ������ �ִ� �ֵ�

    [SerializeField]
    private int _spawnCount = 0; // ó���� �� ��?
    [SerializeField]
    private float _trailSpawnTime = 0.2f; // ���� �ֱ�
    [SerializeField]
    private float _fadeDuration = 0.5f;
    private float _spawnTimer = 0f;

    [SerializeField]
    private bool _isMotionTrail = false;
    public bool IsMotionTrail { get => _isMotionTrail; set => _isMotionTrail = value; }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
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
        materialUpdate(renderer, 1f, _startColor, _startColor);
    }

    private void Update()
    {
        if (_isMotionTrail == false)
            return;

        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= _trailSpawnTime) // ��ȯ��
        {
            Debug.Log("�����");
            _spawnTimer = 0f;
            PopTrail();
        }
    }

    private void PopTrail()
    {
        if (_readyTrails.Count == 0) // ���� �����
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
            Color color = Color.Lerp(_endColor, _startColor, time);
            materialUpdate(renderer, time, color, color);
            time -= Time.deltaTime * (1 / _fadeDuration);
            yield return null;
        }

        _enalbeTrails.Dequeue();
        trail.myObj.SetActive(false);
        _readyTrails.Enqueue(trail);
        yield break;
    }

    private void materialUpdate(MeshRenderer renderer, float alpha, Color fresnelColor, Color baselColor)
    {
        renderer.materials[0].SetFloat("_Alpha", alpha);
        renderer.materials[0].SetColor("_FresnelColor", fresnelColor);
        renderer.materials[0].SetColor("_BaselColor", baselColor);
    }
}
