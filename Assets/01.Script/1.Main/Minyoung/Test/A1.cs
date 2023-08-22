using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class A1 : MonoSingleTon<A1>
{
    [SerializeField] private Transform parent;

    [SerializeField] private GameObject gimmickTem;

    [SerializeField] private GimmickEncyclopediaSO gimmickEncyclopediaSO;

    public Animator animator;
    public TextMeshProUGUI gimmickName;
    public TextMeshProUGUI gimmickExplain;


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
            obj.GetComponent<GimmickInfoGIF>().gimmickSO = gimmickEncyclopediaSO.gimmickEncyclopedia[i];
        }
    }
    public void OnGIF()
    {

        //gimmickName.text = 
        //gimmickExplain.text = 
    }
}
