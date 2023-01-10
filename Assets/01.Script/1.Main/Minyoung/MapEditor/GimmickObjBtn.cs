using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GimmickObjBtn : MonoBehaviour
{
    private GameObject myObj;

    public GimmickSpriteSO so;
    public int i;
    private void Start()
    {
       // myObj = MapDrawManager.Instance.gimmickBtns();
        transform.GetComponent<Button>().onClick.AddListener(() =>
        {
            Select();
        });
    }

    private void Select()
    {
        GameObject clickObj = EventSystem.current.currentSelectedGameObject;

        MapDrawManager.Instance.CurrentSelectSprite = clickObj.GetComponent<Image>().sprite;
        MapDrawManager.Instance.OnMapObj = so.gimmickObj[i];

        Debug.Log("���� �׷����� ��������Ʈ��" + MapDrawManager.Instance.CurrentSelectSprite);
    }

}
