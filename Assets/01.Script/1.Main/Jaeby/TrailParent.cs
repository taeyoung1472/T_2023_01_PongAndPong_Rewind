using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailParent : MonoBehaviour
{
    private TrailableData _data = null;
    public TrailableData Data => _data;
    private TrailableObject _trailableObject = null;

    private bool _died = false;
    public bool Died { get => _died; set => _died = value; }

    private Queue<MeshTrailStruct> _readyTrails = new Queue<MeshTrailStruct>(); // 나올 수 있는 애들
    private Queue<MeshTrailStruct> _enalbeTrails = new Queue<MeshTrailStruct>(); // 꺼지고 있는 애들

    private float _spawnTimer = 0f;
    public float SpawnTimer { get => _spawnTimer; set => _spawnTimer = value; }
    private float _trailSpawnTime = 0f;

    private MeshFilter _meshFilter = null;

    public void Init(TrailableObject obj)
    {
        _trailableObject = obj;
        _data = obj.trailData;
        _trailSpawnTime = _data.trailSpawnTime;
    }

    public void ForceDeleteTrail()
    {
        StopAllCoroutines();
        while (_enalbeTrails.Count != 0)
        {
            MeshTrailStruct trail = _enalbeTrails.Dequeue();
            trail.myObj.SetActive(false);
            _readyTrails.Enqueue(trail);
        }
    }

    private void SpawnTrail()
    {
        MeshTrailStruct trail = TrailManager.Instance.SpawnTrail(this);
        _readyTrails.Enqueue(trail);
        trail.BodyMeshFilter = trail.myObj.transform.GetChild(0).GetComponent<MeshFilter>();
        trail.bodyMesh = new Mesh();
        MeshSet(trail);

        MeshRenderer renderer = trail.BodyMeshFilter.GetComponent<MeshRenderer>();
        List<Material> materials = new List<Material>();
        if (_data._skinnedMeshRenderer != null)
        {
            for (int i = 0; i < _data._skinnedMeshRenderer.materials.Length; i++)
                materials.Add(_data.trailMaterial);
        }
        else if (_data._meshRenderer != null)
        {
            for (int i = 0; i < _data._meshRenderer.materials.Length; i++)
                materials.Add(_data.trailMaterial);
        }
        renderer.materials = materials.ToArray();
        materialUpdate(renderer, 1f, _data.startFresnelColor, _data.startBaseColor);
    }

    private void PopTrail()
    {
        if (_readyTrails.Count == 0) // 새로 만들엉
        {
            SpawnTrail();
        }

        MeshTrailStruct trail = _readyTrails.Dequeue();
        trail.bodyMesh = new Mesh();
        MeshSet(trail);
        trail.myObj.SetActive(true);
        trail.myObj.transform.SetPositionAndRotation(_data.spawnTrm.position, _data.spawnTrm.rotation);
        StartCoroutine(FadeCoroutine(trail));
    }

    private void MeshSet(MeshTrailStruct trail)
    {
        if (_data._skinnedMeshRenderer != null)
        {
            _data._skinnedMeshRenderer.BakeMesh(trail.bodyMesh);
            trail.BodyMeshFilter.mesh = trail.bodyMesh;
        }
        else if (_data._meshRenderer != null)
        {
            if (_meshFilter == null)
                _meshFilter = _data._meshRenderer.GetComponent<MeshFilter>();
            trail.BodyMeshFilter.mesh = _meshFilter.mesh;
        }
    }

    private IEnumerator FadeCoroutine(MeshTrailStruct trail)
    {
        _enalbeTrails.Enqueue(trail);
        float time = 1f;
        while (time >= 0f)
        {
            MeshRenderer renderer = trail.BodyMeshFilter.GetComponent<MeshRenderer>();
            Color fresnelColor = Color.Lerp(_data.endFresnelColor, _data.startFresnelColor, time);
            Color baseColor = Color.Lerp(_data.endBaseColor, _data.startBaseColor, time);
            materialUpdate(renderer, time, fresnelColor, baseColor);
            time -= Time.deltaTime * (1 / _data.fadeDuration);
            yield return null;
        }

        _enalbeTrails.Dequeue();
        trail.myObj.SetActive(false);
        _readyTrails.Enqueue(trail);
        yield break;
    }

    private void materialUpdate(MeshRenderer renderer, float alpha, Color fresnelColor, Color baseColor)
    {
        for (int i = 0; i < renderer.materials.Length; i++)
        {
            renderer.materials[i].SetFloat("_Alpha", alpha);
            renderer.materials[i].SetColor("_FresnelColor", fresnelColor);
            renderer.materials[i].SetColor("_BaseColor", baseColor);
        }
    }

    public void ParentUpdate()
    {
        if (_trailableObject.IsMotionTrail == false || _died)
            return;

        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= _trailSpawnTime) // 소환띠
        {
            _spawnTimer = 0f;
            PopTrail();
        }
    }
}
