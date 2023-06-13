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
 
    public void OnPointerEnter(PointerEventData eventData)
    {
        //PhoneStage.Instance.ExplainUI.gameObject.SetActive(true);

        //PhoneStage.Instance.ExplainUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = gimmickInfoSO.gimmickName;
        //PhoneStage.Instance.ExplainUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = gimmickInfoSO.gimmickExplain;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
        Debug.Log(mousePos);
        PhoneStage.Instance.ExplainUI.anchoredPosition = mousePos;
       

        Debug.Log("마우스올라감");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //PhoneStage.Instance.ExplainUI.gameObject.SetActive(false);

    }
}
