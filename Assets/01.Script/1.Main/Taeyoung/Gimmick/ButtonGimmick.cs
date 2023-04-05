using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ButtonGimmick : MonoBehaviour
{
    [SerializeField] private bool isTogle;
    private bool toggleFlag = false;

    bool isActive = false;

    [SerializeField] private ControlData[] controlDataArr;
    [SerializeField] private GimmickVisualLink visualLinkPrefab;
    [SerializeField] private Color color =  Color.white;

    [ContextMenu("Gen Color")]
    public void GenColor()
    {
        color = Random.ColorHSV();
    }
    public void Reset()
    {
        GenColor();
    }

    public void Start()
    {
        foreach (var data in controlDataArr)
        {
            GimmickVisualLink link = Instantiate(visualLinkPrefab);
            link.Link(transform, data.target.transform, color);
        }
        if (RewindManager.Instance)
        {
            RewindManager.Instance.RestartPlay += () =>
            {
                isActive = false;
            };
        }
    }

    public void Update()
    {
        CheckPlayer();

        if (isActive)
        {
            foreach (var control in controlDataArr)
            {
                control.target.Control(control.isReverse ? ControlType.ReberseControl : ControlType.Control);
            }
        }
        else
        {
            foreach (var control in controlDataArr)
            {
                control.target.Control(ControlType.None);
                CamManager.Instance.RemoveTargetGroup(control.target.transform);
            }
        }
        //if (isTogle)
        //{
        //    if (toggleFlag)
        //    {
        //        foreach (var control in controlDataArr)
        //        {
        //            CamManager.Instance.AddTargetGroup(control.target.transform);
        //        }
        //    }
        //    else
        //    {

        //        foreach (var control in controlDataArr)
        //        {
        //            CamManager.Instance.RemoveTargetGroup(control.target.transform);
        //        }
        //    }
        //}
    }

    private void CheckPlayer()
    {
        if (isActive)
            return;

        foreach (var col in Physics.OverlapBox(transform.position, Vector3.one * 2.5f))
        {
            if (col.gameObject.TryGetComponent<Player>(out Player player))
            {
                foreach (var control in controlDataArr)
                {
                    CamManager.Instance.AddTargetGroup(control.target.transform);
                }

                return;
            }
        }

        foreach (var control in controlDataArr)
        {
            CamManager.Instance.RemoveTargetGroup(control.target.transform);
        }
        return;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<GimmickObject>(out GimmickObject gimmickObject))
        {
            isActive = true;
        }
        
        //if (!isTogle) return;
        //toggleFlag = !toggleFlag;
        //if (other.TryGetComponent<GimmickObject>(out GimmickObject gimmickObject))
        //{
        //    foreach (var control in controlDataArr)
        //    {
        //        if(!toggleFlag)
        //        {
        //            control.target.Control(ControlType.None);
        //        }
        //        else
        //        {
        //            control.target.Control(control.isReverse ? ControlType.ReberseControl : ControlType.Control);
        //        }
        //    }
        //}
    }

    //public void OnTriggerStay(Collider other)
    //{
    //    if (isTogle) return;

    //    if (other.TryGetComponent<GimmickObject>(out GimmickObject gimmickObject))
    //    {
    //        foreach (var control in controlDataArr)
    //        {
    //            control.target.Control(control.isReverse ? ControlType.ReberseControl : ControlType.Control);
    //            CamManager.Instance.AddTargetGroup(control.target.transform);
    //            isActive = true;
    //        }
    //    }
    //}
    public void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<GimmickObject>(out GimmickObject gimmickObject))
        {
            isActive = false;
        }
        //if (isTogle) return;

        //if (other.TryGetComponent<GimmickObject>(out GimmickObject gimmickObject))
        //{
        //    foreach (var control in controlDataArr)
        //    {
        //        control.target.Control(ControlType.None);
        //        CamManager.Instance.RemoveTargetGroup(control.target.transform);
        //        isActive = false;
        //    }
        //}
    }

    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
        
    //}

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        foreach (var control in controlDataArr)
        {
            if (control == null)
                continue;

            Handles.color = control.isReverse ? Color.red : Color.blue;
            Handles.DrawLine(transform.position, control.target.transform.position, 10);
        }
    }
#endif

    [Serializable]
    private class ControlData
    {
        public ControlAbleObjcet target;
        public bool isReverse = true;
    }
}
