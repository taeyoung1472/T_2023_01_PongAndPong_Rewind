using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StageWorldSeletManager : MonoBehaviour
{
    [SerializeField]
    private StageWorldListSO _stageWorldListSO;
    [SerializeField]
    private Transform _parentTrm = null;
    [SerializeField]
    private Button _stageWorldSelectButtonPrefab = null;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        StageWorldSelectData.curStageWorld = null;

        for (int i = 0; i < _stageWorldListSO.stageWorlds.Count; i++)
        {
            Button obj = Instantiate(_stageWorldSelectButtonPrefab, _parentTrm);
            TextMeshProUGUI text = obj.GetComponentInChildren<TextMeshProUGUI>();
            text.SetText($"Stage{i}");
            ButtonSet(obj, i);
        }
    }

    public void ButtonSet(Button button, int worldIndex)
    {
        button.onClick.AddListener(() =>
        {
            StageWorldSelectData.curStageWorld = _stageWorldListSO.stageWorlds[worldIndex];
            SceneManager.LoadScene("SceneSelectTestScene");
        });
    }
}
