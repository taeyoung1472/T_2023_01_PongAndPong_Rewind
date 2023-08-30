using DigitalRuby.ThunderAndLightning;
using EPOOutline;
using System.Collections.Generic;
using UnityEngine;

public class GimmickVisualLink : MonoBehaviour
{
    [SerializeField] private LineRenderer linkRenderer;
    [SerializeField, Range(1, 10)] private int pivotCount = 4;
    [HideInInspector] public List<Vector3> pivots = new();
    [HideInInspector] public Color color;
    private Material lineMaterial;
    private float worldZ = 1.9f;

    #region RunTime
    private void Awake()
    {
        lineMaterial = linkRenderer.materials[0];
    }

    public void Active(bool isActive)
    {
        if (isActive)
            lineMaterial.SetColor("_EmissionColor", color * 1.75f);
        else
            lineMaterial.SetColor("_EmissionColor", color * 0.1f);
    }
    #endregion

    #region InEditor
    public void Reset()
    {
        float randomWorldZ = worldZ + Random.Range(-0.01f, 0.01f);
        pivots = new(pivotCount);
        for (int i = 0; i < pivotCount; i++)
        {
            if (i == 0)
            {
                pivots.Add(new Vector3((int)transform.position.x, (int)transform.position.y, 0));
            }
            else
            {
                Vector3 newPos = pivots[i - 1] + Vector3.up;
                pivots.Add(new Vector3((int)newPos.x, (int)newPos.y, 0));
            }
            pivots[i] = new Vector3(pivots[i].x, pivots[i].y, randomWorldZ);
        }
    }

    public void Generate()
    {
        linkRenderer.positionCount = pivots.Count;
        linkRenderer.SetPositions(pivots.ToArray());
    }
    #endregion
}
