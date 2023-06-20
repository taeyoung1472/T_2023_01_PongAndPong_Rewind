using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhoneGimmick : MonoSingleTon<PhoneGimmick>
{
    [SerializeField] private Transform parent;

    [SerializeField] private GameObject gimmickTem;

    [SerializeField] private GimmickEncyclopediaSO gimmickEncyclopediaSO;
    public void Start()
    {
        CreateTem();   
    }
    public void CreateTem()
    {
        for (int i = 0; i < gimmickEncyclopediaSO.gimmickEncyclopedia.Count; i++)
        {
            GameObject obj = Instantiate(gimmickTem, parent);
            obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = gimmickEncyclopediaSO.gimmickEncyclopedia[i].gimmickName;
            obj.transform.GetChild(1).GetComponent<Image>().sprite = gimmickEncyclopediaSO.gimmickEncyclopedia[i].gimmickIcon;

        }
    }
    public void OnStageGimmick()
    {
        
    }

}