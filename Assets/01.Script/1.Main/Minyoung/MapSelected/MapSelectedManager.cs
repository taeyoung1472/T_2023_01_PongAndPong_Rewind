using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MapSelectedManager : MonoBehaviour
{
    [SerializeField] private GameObject stageSelectPanel;

    [SerializeField] private Button preStageBtn;
    [SerializeField] private Button nextStageBtn;
    [SerializeField] private MapPrefabSO mapSO;
    [SerializeField] private TextMeshProUGUI stageExplain;

    public int stageIndex = 0;

    public bool stageSelected = false;
    void Start()
    {
        stageIndex = 0;

        BtnSet(preStageBtn, false);
        BtnSet(nextStageBtn, true);
    }

    public void BtnSet(Button btn, bool next)
    {
        btn.onClick.AddListener(() =>
        {
            if (stageSelected)
            {
                if (next)
                {
                    stageIndex += 1;
                }
                else
                {
                    stageIndex -= 1;
                }
                stageIndex = Mathf.Clamp(stageIndex, 0, mapSO.map.Count - 1);

                stageExplain.text = mapSO.map[stageIndex].stageInfo;
                StageWorldSelectData.curStageWorld = mapSO.map[stageIndex];
            }
            stageSelected = !stageSelected;
        });
    }

    public void PanelDown()
    {
        stageSelectPanel.SetActive(false);
        stageIndex = 0;
    }

    public void SceneChange()
    {
        LoadingSceneManager.LoadScene(2);
    }
}
