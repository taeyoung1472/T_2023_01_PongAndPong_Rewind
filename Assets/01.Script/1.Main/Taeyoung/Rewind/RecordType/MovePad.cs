using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MovePad : TransformRecord
{
    [SerializeField] private Transform targetA;
    [SerializeField] private Transform targetB;

    [SerializeField] private Color colorA;
    [SerializeField] private Color colorB;

    [SerializeField] private TextMeshPro infoTmp;

    MeshRenderer meshRenderer;

    private List<Color> colorList = new();
    private List<float> valueList = new();

    float value;

    public override void OnUpdate()
    {
        base.OnUpdate();

        transform.position = Vector3.Lerp(targetA.position, targetB.position, (Mathf.Sin(Time.time) / 2) + 0.5f);
        meshRenderer.material.color = Color.Lerp(colorA, colorB, (Mathf.Sin(Time.time) / 2) + 0.5f);

        value = (Mathf.Sin(Time.time) / 2) + 0.5f;

        infoTmp.text = $"{value:0.0}";
    }

    public override void Register()
    {
        base.Register();

        meshRenderer = GetComponent<MeshRenderer>();

        colorList.Capacity = TotalRecordCount;
        colorList.AddRange(new Color[TotalRecordCount]);
        colorList[InitIndex] = meshRenderer.material.color;

        valueList.Capacity = TotalRecordCount;
        valueList.AddRange(new float[TotalRecordCount]);
        valueList[InitIndex] = 0;
    }

    public override void Recorde(int index)
    {
        base.Recorde(index);
        colorList[index] = meshRenderer.material.color;

        valueList[index] = value;
    }

    public override void ApplyData(int index, int nextIndexDiff)
    {
        base.ApplyData(index, nextIndexDiff);

        index = Mathf.Clamp(index, 0, TotalRecordCount - 1);
        int nextIndex = Mathf.Clamp(index + nextIndexDiff, 0, TotalRecordCount - 1);

        meshRenderer.material.color = Color.Lerp(colorList[index], colorList[nextIndex], RecordingPercent);

        infoTmp.text = $"{Mathf.Lerp(valueList[index], valueList[nextIndex], RecordingPercent):0.0}";
    }
}
