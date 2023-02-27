using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/ObjectDatabase")]
public class EditorObjectDataBase : ScriptableObject
{
    public List<EditorObjectData> objectList;

    public void OnValidate()
    {
        var sortedList = objectList.OrderBy(x => x.objectType);
        objectList = sortedList.ToList();
    }
}
