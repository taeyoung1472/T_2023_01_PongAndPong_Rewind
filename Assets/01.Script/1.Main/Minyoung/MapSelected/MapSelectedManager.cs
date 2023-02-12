using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapSelectedManager : MonoBehaviour
{
    // 12 편ㅇㅢㅈㅓㅁ가고 라며ㄴ 삼ㄱㅏㄱㄱㅂㅏㅂ 핫ㅂㅏ 
    //1ㅅ시   에디터 wasd 끝 

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

        //preStageBtn.onClick.AddListener(() =>
        //{
        //    stageIndex -= 1;
        //    StageWorldSelectData.curStageWorld = mapSO.map[stageIndex];
        //    Debug.Log(mapSO.map[stageIndex]);
        //});
        //nextStageBtn.onClick.AddListener(() =>
        //{
        //    if (stageSelected)
        //    {
        //        stageIndex += 1;
        //        stageExplain.text = mapSO.map[stageIndex].stageInfo;
        //        StageWorldSelectData.curStageWorld = mapSO.map[stageIndex];
        //        Debug.Log(mapSO.map[stageIndex]);
        //    }
        //    stageSelected = !stageSelected;
        //});
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

                stageExplain.text = mapSO.map[stageIndex].stageInfo;
                StageWorldSelectData.curStageWorld = mapSO.map[stageIndex];
                Debug.Log(mapSO.map[stageIndex]);
            }

            stageSelected = !stageSelected;

         
        });
      
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            stageSelectPanel.SetActive(true);
        }
    }

}
