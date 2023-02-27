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

        bool isLock = false;

        for (int i = 0; i < _stageWorldListSO.stageWorlds.Count; i++)
        {
            Button obj = Instantiate(_stageWorldSelectButtonPrefab, _parentTrm);
            TextMeshProUGUI text = obj.GetComponentInChildren<TextMeshProUGUI>();

            if (isLock)
            {
                obj.interactable = false;
                text.SetText($"L O C K");
            }
            else
            {
                text.SetText($"Stage {i + 1}");
            }

            if (!_stageWorldListSO.stageWorlds[i].isClear)
            {
                isLock = true;
            }

            ButtonSet(obj, i);
        }
    }

    public void ButtonSet(Button button, int worldIndex)
    {
        button.onClick.AddListener(() =>
        {
            StageWorldSelectData.curStageWorld = _stageWorldListSO.stageWorlds[worldIndex];
            LoadingSceneManager.LoadScene(2);
        });
    }
}
