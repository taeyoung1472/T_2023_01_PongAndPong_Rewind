using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentGimmick : GimmickObject
{
    [SerializeField] private float disableTime;
    private bool isStart = false;
    public GameObject childObj;

    public override void Awake()
    {
        base.Awake();
    }
    
    public override void InitOnPlay()
    {
        disableTime = 0f;
        childObj.gameObject.SetActive(true);
        isStart = true;
    }
    public override void InitOnRewind()
    {
        Debug.Log(RewindManager.Instance.howManySecondsToTrack - disableTime);
        StartCoroutine(ActiveChild(RewindManager.Instance.howManySecondsToTrack - disableTime));
    }
    void Update()
    {
        if (isStart)
        {
            disableTime += Time.deltaTime;
        }
        if (!childObj.gameObject.activeSelf)
        {
            isStart = false;
        }
    }  
    IEnumerator ActiveChild(float time)
    {
        yield return new WaitForSeconds(time);
        childObj.gameObject.SetActive(true);
        Debug.Log(childObj.gameObject.activeSelf);
    }

    public override void Init()
    {
    }
}
