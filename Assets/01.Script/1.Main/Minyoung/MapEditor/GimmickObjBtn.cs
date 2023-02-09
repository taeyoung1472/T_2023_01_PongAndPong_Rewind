using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GimmickObjBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button myButton;
    [SerializeField] private Image contentImage;

    public GimmickInfoSO gimmickInfo;


    private string myObjNameStr;
    public void Init()
    {

        myButton = GetComponent<Button>();
        contentImage.sprite = gimmickInfo.sprite;
        myObjNameStr = gimmickInfo.nameStr;

        myButton.onClick.AddListener(() =>
        {
            MapDrawManager.Instance.OnMapObj = gimmickInfo.prefab;

            MapDrawManager.Instance.SetGhostObject(gimmickInfo.prefab);
        });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MapDrawManager.Instance.explainTab.transform.position =  transform.position;


        MapDrawManager.Instance.explainTab.SetActive(true);
        MapDrawManager.Instance.explainTab.GetComponentInChildren<TextMeshProUGUI>().text = myObjNameStr;


    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MapDrawManager.Instance.explainTab.SetActive(false);
    }
}


