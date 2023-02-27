using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MapSelectedManager : MonoBehaviour
{
    [SerializeField] private GameObject stageSelectPanel;

    [SerializeField] private MapPrefabSO mapSO;
    [SerializeField] private TextMeshProUGUI stageExplain;

    public int stageIndex = 0;

    public bool stageSelected = false;
    void Start()
    {
        StageChange(0);
    }

    public void PanelDown()
    {
        stageSelectPanel.SetActive(false);
        StageChange(0);
    }

    public void NextStage()
    {
        StageChange(stageIndex + 1);
    }

    public void PrevStage()
    {
        StageChange(stageIndex - 1);
    }

    private void StageChange(int index)
    {
        if (index < 0)
            index = mapSO.map.Count - 1;
        stageIndex = index % mapSO.map.Count;
        stageExplain.SetText(mapSO.map[stageIndex].stageInfo);
        StageWorldSelectData.curStageWorld = mapSO.map[stageIndex];
    }

    public void SceneChange()
    {
        DOTween.KillAll();
        LoadingSceneManager.LoadScene(2);
    }

    public void DDD(Vector2 a)
    {
        Debug.Log(a);
    }
}
