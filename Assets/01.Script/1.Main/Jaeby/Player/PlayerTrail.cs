using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrail : MonoBehaviour
{
    [SerializeField]
    private Transform _spawnTrm = null; // ��� ������?

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

        //float alphaVal = (1f - (float)i / TrailCount) * 0.5f;
        pss.BodyMeshFilter.GetComponent<MeshRenderer>().material.SetFloat("_Alpha", 1f);

        //Color tmpColor = Color.Lerp(frontColor, backColor, (float)i / TrailCount);
        //pss.BodyMeshFilter.GetComponent<MeshRenderer>().material.SetColor("_FresnelColor", tmpColor);

        //Color tmpColor_Inner = Color.Lerp(frontColor_Inner, backColor_Inner, (float)i / TrailCount);
        //pss.BodyMeshFilter.GetComponent<MeshRenderer>().material.SetColor("_BaselColor", tmpColor_Inner);
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
        trail.myObj.SetActive(true);
        trail.myObj.transform.SetPositionAndRotation(_spawnTrm.position, _spawnTrm.rotation);
        StartCoroutine(FadeCoroutine(trail));
    }

    public void StartTrail()
    {
        _isMotionTrail = true;
    }

    public void EndTrail()
    {
        _isMotionTrail = false;
    }

    private IEnumerator FadeCoroutine(MeshTrailStruct trail)
    {
        _enalbeTrails.Enqueue(trail);
        //���̵� to do
        yield return new WaitForSeconds(1.5f);
        _enalbeTrails.Dequeue();
        trail.myObj.SetActive(false);
        _readyTrails.Enqueue(trail);
        yield break;
    }
}
