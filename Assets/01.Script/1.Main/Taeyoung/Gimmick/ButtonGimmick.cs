using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
public class ButtonGimmick : MonoBehaviour
{
    [SerializeField] private bool isTogle;
    //private bool toggleFlag = false;

    bool isActive = false;

    public bool isRewind = false;
    [SerializeField] private ControlData[] controlDataArr;
    [SerializeField] private GimmickVisualLink visualLinkPrefab;
    [SerializeField] private Color color = Color.white;
    private Transform map;

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
        if (RewindManager.Instance)
        {
            RewindManager.Instance.RestartPlay += () =>
            {
                foreach (var control in controlDataArr)
                {
                    control.target.isLocked = false; 
                }
                isActive = false;
            };
            RewindManager.Instance.InitPlay += () =>
            {
                isRewind = false;
            };
            RewindManager.Instance.InitRewind += () =>
            {
                isRewind = true;
            };
        }
        foreach (var data in controlDataArr)
        {
            GimmickVisualLink link = Instantiate(visualLinkPrefab);
            link.Link(transform, data.target.transform, color);
        }
      
    }

    public void Update()
    {
        if (isRewind)
        {
            return;
        }

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
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<GimmickObject>(out GimmickObject gimmickObject))
        {
            isActive = false;
        }
    }

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
        public bool isDG = true;
    }
}
