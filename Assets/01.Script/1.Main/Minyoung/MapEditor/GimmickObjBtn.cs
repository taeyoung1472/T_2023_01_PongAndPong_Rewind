using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GimmickObjBtn : MonoBehaviour
{
    public GimmickSpriteSO so;
    public int i;
    private void Start()
    {
       // myObj = MapDrawManager.Instance.gimmickBtns();
        transform.GetComponent<Button>().onClick.AddListener(() =>
        {
            MapDrawManager.Instance.OnMapObj = so.gimmickObj[i];
            //Select();
        });
    }

    private void Select()
    {
      //  GameObject clickObj = EventSystem.current.currentSelectedGameObject;

      //  MapDrawManager.Instance.CurrentSelectSprite = clickObj.GetComponent<Image>().sprite;

        //Debug.Log("���� �׷����� ��������Ʈ��" + MapDrawManager.Instance.CurrentSelectSprite);
    }
}


