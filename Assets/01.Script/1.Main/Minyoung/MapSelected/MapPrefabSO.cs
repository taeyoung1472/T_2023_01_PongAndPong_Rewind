using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MapPrefab")]
public class MapPrefabSO : ScriptableObject
{
    public List<StageDataSO> map;
}
