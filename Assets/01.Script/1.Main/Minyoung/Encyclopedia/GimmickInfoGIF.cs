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
        Debug.Log("喚顕");

        PhoneGimmick.Instance.scrollview.gameObject.SetActive(false);
        PhoneGimmick.Instance.backImage.gameObject.SetActive(false);
        PhoneGimmick.Instance.gifMenu.gameObject.SetActive(true);

        PhoneGimmick.Instance.gimmickName.text = gimmickSO.gimmickName;
        PhoneGimmick.Instance.gimmickExplain.text = gimmickSO.gimmickExplain;
        PhoneGimmick.Instance.animator.Play(gimmickSO.animName);

    }
    public void PushCoumpter()
    {
        A1.Instance.gimmickName.text = gimmickSO.gimmickName;
        A1.Instance.gimmickExplain.text = gimmickSO.gimmickExplain;
        A1.Instance.animator.Play(gimmickSO.animName);
    }
    public void PushStageInfo()
    {

        Debug.Log("Sしいしいいけしいげ");
        UIManager.Instance.BackPressDirector(UIManager.Instance.stageImg.transform);
        UIManager.Instance.PressDirector(UIManager.Instance.gimmickImg.transform);

       // UIManager.Instance.currentOpenImg = UIManager.Instance.gimmickImg;

       // UIManager.Instance.setActiveFalseObjs[2].SetActive(true);
        //UIManager.Instance.PressDirector(UIManager.Instance.setActiveFalseObjs[2].transform);

        PhoneGimmick.Instance.scrollview.gameObject.SetActive(false);
        PhoneGimmick.Instance.backImage.gameObject.SetActive(false);
        PhoneGimmick.Instance.gifMenu.gameObject.SetActive(true);

        PhoneGimmick.Instance.gimmickName.text = gimmickSO.gimmickName;
        PhoneGimmick.Instance.gimmickExplain.text = gimmickSO.gimmickExplain;
        PhoneGimmick.Instance.animator.Play(gimmickSO.animName);


    }
    void Start()
    {
        btn = transform.GetComponent<Button>();
    }

    void Update()
    {
        
    }
}
