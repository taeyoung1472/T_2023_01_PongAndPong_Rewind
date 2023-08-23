using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LabCollectionObj : MonoBehaviour
{
    [SerializeField]
    private WorldDataSO _myWorldData = null;
    [SerializeField]
    private TextMeshProUGUI _percentText = null;
    [SerializeField]
    private float _textAnimatingTime = 5f;
    private DissolveAnimator _dissolveAnimator = null;

    [SerializeField] private int index = 0;

    public void CollectPercentSet()
    {
        if (_myWorldData == null)
            return;

        if (_dissolveAnimator == null)
            _dissolveAnimator = GetComponent<DissolveAnimator>();


        int currentCollection = SaveDataManager.Instance.CurrentChapterCollectionCount(_myWorldData.worldName, index);

        //StageCollectionData stageCollectionData = SaveDataManager.Instance.AllChapterDataBase.stageCollectionDataDic[_myWorldData.worldName]
        //    .stageCollectionValueList[_myWorldData.stageList[0].stageIndex];
        //StageCollectionData stageCollectionDatsa = SaveDataManager.Instance.AllChapterDataBase.stageCollectionDataDic[_myWorldData.worldName]
        // .stageCollectionValueList[_myWorldData.stageList.Count];
        //int currentCollection = 0;

        //foreach (var e in stageCollectionData.stageDataList)
        //{
        //    currentCollection += e.zoneCollections.collectionBoolList.FindAll(x => x == true).Count;
        //}

        int maxCollection = SaveDataManager.Instance.MaxChapterCollectionCount(_myWorldData.worldName, index);
        //int maxCollection = stageCollectionData.stageDataList.Count;

        Debug.Log(_myWorldData.worldName + "  current : " + currentCollection + "  max : " + maxCollection);
        float ratio = ((float)currentCollection/ maxCollection);
        _dissolveAnimator.DissolveStart(_dissolveAnimator.GetDissolveRatio(), ratio, new Vector3(0f, -1f, 0f));
        TextAnimating((int)(ratio * 100f));
    }

    private void TextAnimating(int endVal)
    {
        StartCoroutine(TextAnimatingCoroutine(endVal));
    }

    private IEnumerator TextAnimatingCoroutine(int endVal)
    {
        float time = 0f;
        while (time <= 1f)
        {
            _percentText.SetText($"{(endVal * time).ToString("N0")}%");
            time += Time.deltaTime * (1 / _textAnimatingTime);
            yield return null;
        }
        _percentText.SetText($"{(endVal).ToString("N0")}%");
    }
}
