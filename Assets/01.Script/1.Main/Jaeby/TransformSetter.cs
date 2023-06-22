using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformSetter : MonoBehaviour
{
    [SerializeField]
    private List<TransformSetData> _positionSetDatas = new List<TransformSetData>();

    private void Awake()
    {
        Setting(TransformSetType.Awake);
    }

    private void Start()
    {
        Setting(TransformSetType.Start);
    }

    private void Setting(TransformSetType type)
    {
        foreach (var data in _positionSetDatas)
        {
            if (data.settingTimeType == type)
            {
                data.targetTrm.SetPositionAndRotation(data.settingTrm.position, data.settingTrm.rotation); ;
                data.targetTrm.localScale = data.settingTrm.localScale;
            }
        }
    }
}

[System.Serializable]
public class TransformSetData
{
    public TransformSetType settingTimeType = TransformSetType.None;
    public Transform targetTrm;
    public Transform settingTrm;
}