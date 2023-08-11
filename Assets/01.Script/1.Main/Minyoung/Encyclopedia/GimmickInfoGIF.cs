using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GimmickInfoGIF : MonoBehaviour
{
    
    public GimmickInfoSO gimmickSO;
    public Button btn;
    public void PushBtn()
    {
        PhoneGimmick.Instance.scrollview.gameObject.SetActive(false);
        PhoneGimmick.Instance.backImage.gameObject.SetActive(false);
        PhoneGimmick.Instance.gifMenu.gameObject.SetActive(true);

        Debug.Log(gimmickSO);
        PhoneGimmick.Instance.gimmickName.text = gimmickSO.gimmickName;
        PhoneGimmick.Instance.gimmickExplain.text = gimmickSO.gimmickExplain;
        Debug.Log(PhoneGimmick.Instance.animator);
        PhoneGimmick.Instance.animator.Play(gimmickSO.animName);
       

    }
    void Start()
    {
        btn = transform.GetComponent<Button>();
        Debug.Log("¾Æ¾Æ");

    }

    void Update()
    {
        
    }
}
