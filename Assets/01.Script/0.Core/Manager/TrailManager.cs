using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailManager : MonoSingleTon<TrailManager>
{
    private Dictionary<TrailableObject, TrailParent> _trailParents = new();
    private Transform _rootTrm = null;

    [SerializeField]
    private int _spawnCount = 0; // 처음에 몇 개?
    [SerializeField]
    private GameObject _meshTrailPrefab = null;

    public void TrailTimerReset(TrailableObject obj)
    {
        if (_trailParents.ContainsKey(obj))
        {
            _trailParents[obj].SpawnTimer = 0f;
        }
    }

    public void AddTrailObj(TrailableObject obj)
    {
        if(_trailParents.ContainsKey(obj) == false)
        {
            Transform parentTrm = new GameObject($"Parent : {obj.gameObject.name}").transform;
            parentTrm.SetParent(_rootTrm);
            TrailParent parent = parentTrm.gameObject.AddComponent<TrailParent>();
            parent.Init(obj);
            _trailParents.Add(obj, parent);
            for (int i = 0; i < _spawnCount; i++)
            {
                SpawnTrail(parent);
            }
        }
    }

    public MeshTrailStruct SpawnTrail(TrailParent parent)
    {
        MeshTrailStruct pss = new MeshTrailStruct();
        pss.myObj = Instantiate(_meshTrailPrefab, parent.transform);
        pss.myObj.SetActive(false);
        return pss;
    }

    public void DeleteTrailObj(TrailableObject obj)
    {
        if(_trailParents.ContainsKey(obj))
        {
            _trailParents[obj].Died = true;
            _trailParents.Remove(obj);
        }
    }

    private void Awake()
    {
        _rootTrm = new GameObject("@ROOT").transform;
        _rootTrm.SetParent(transform);
    }

    private void Update()
    {
        foreach(var parent in _trailParents)
        {
            parent.Value.ParentUpdate();
        }
    }
}

[System.Serializable]
public class TrailableData
{
    public Transform spawnTrm = null; // 어디에 생성띠?
    public Material trailMaterial = null;
    [ColorUsage(true, true)]
    public Color startFresnelColor = Color.white;
    [ColorUsage(true, true)]
    public Color startBaseColor = Color.white;
    [ColorUsage(true, true)]
    public Color endFresnelColor = Color.white;
    [ColorUsage(true, true)]
    public Color endBaseColor = Color.white;
    public float trailSpawnTime = 0.2f; // 생성 주기
    public float fadeDuration = 0.5f;
    public SkinnedMeshRenderer _skinnedMeshRenderer = new(); // ㅎㅎ
    public MeshRenderer _meshRenderer = new();
}
