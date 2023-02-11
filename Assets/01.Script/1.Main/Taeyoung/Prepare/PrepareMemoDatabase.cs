using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/MemoDatabase")]
public class PrepareMemoDatabase : ScriptableObject
{
    public List<PrepareMemoData> memoList;

    public void OnValidate()
    {
        var sortedList = memoList.OrderBy(x => x.memoType);
        memoList = sortedList.ToList();
    }
}
