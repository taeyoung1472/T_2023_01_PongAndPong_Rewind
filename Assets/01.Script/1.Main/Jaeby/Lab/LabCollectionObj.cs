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
        {
            _percentText.SetText("0%");
            return;
        }

        if (_dissolveAnimator == null)
            _dissolveAnimator = GetComponent<DissolveAnimator>();


        int currentCollection = SaveDataManager.Instance.CurrentChapterCollectionCount(_myWorldData.worldName, index);
        int maxCollection = SaveDataManager.Instance.MaxChapterCollectionCount(_myWorldData.worldName, index);

        Debug.Log(_myWorldData.worldName + "  current : " + currentCollection + "  max : " + maxCollection);
        if((maxCollection + currentCollection) > 0)
        {
            float ratio = ((float)currentCollection / maxCollection);
            _dissolveAnimator.DissolveStart(_dissolveAnimator.GetDissolveRatio(), ratio, new Vector3(0f, -1f, 0f));
            TextAnimating((int)(ratio * 100f));
        }
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
