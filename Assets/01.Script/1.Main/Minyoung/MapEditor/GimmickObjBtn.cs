using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GimmickObjBtn : MonoBehaviour
{
    private Button myButton;
    [SerializeField] private Image contentImage;

    public GimmickInfoSO gimmickInfo;
    private int index;

    public void Init(int idx)
    {
        index = idx;

        myButton = GetComponent<Button>();
        contentImage.sprite = gimmickInfo.sprite;

        myButton.onClick.AddListener(() =>
        {
            MapDrawManager.Instance.OnMapObj = gimmickInfo.prefab;

            MapDrawManager.Instance.SetGhostObject(gimmickInfo.prefab);
            MapDrawManager.Instance.isSelected = true;
        });
    }

    private void Select()
    {
      //  GameObject clickObj = EventSystem.current.currentSelectedGameObject;

      //  MapDrawManager.Instance.CurrentSelectSprite = clickObj.GetComponent<Image>().sprite;

        //Debug.Log("현재 그려지는 스프라이트는" + MapDrawManager.Instance.CurrentSelectSprite);
    }
}


