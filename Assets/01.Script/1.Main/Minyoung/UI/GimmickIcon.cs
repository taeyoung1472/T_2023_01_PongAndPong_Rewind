using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GimmickIcon : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public GimmickInfoSO gimmickInfoSO;
    //public GimmickInfoSO GimmickInfoSO
    //{
    //    get { return gimmickInfoSO; }
    //    set { gimmickInfoSO = value; }
    //}
    private void Update()
    {
        //if (EventSystem.current.IsPointerOverGameObject() == true)
        //{
        //    PhoneStage.Instance.ExplainUI.SetActive(true);
        //    PhoneStage.Instance.ExplainUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = gimmickInfoSO.gimmickName;
        //    PhoneStage.Instance.ExplainUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = gimmickInfoSO.gimmickName;
        //}
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("���콺�ö�");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("���콺������");

        PhoneStage.Instance.ExplainUI.SetActive(false);

    }
}
