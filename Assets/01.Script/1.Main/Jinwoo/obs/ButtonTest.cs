using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ButtonTest// : RewindAbstract
{
//    [Header("[Button]")]
//    private bool isActive = false;
//    [SerializeField] private string buttonName;
//    [SerializeField] private List<GameObject> targetObject;
//    //List<IFunctionalObject> function = new();


//    [SerializeField] bool trackPositionRotation;
//    [SerializeField] bool trackVelocity;
//    [SerializeField] bool trackAnimator;
//    [SerializeField] bool trackAudio;
//    [SerializeField] bool trackParticles;

//    [Tooltip("파티클 추적을 선택한 경우에만 파티클 설정 채우기")]
//    [SerializeField] ParticlesSetting particleSettings;

//    private void OnValidate()
//    {
//        CheckWrongObjFunction();
//    }
//    public void CheckWrongObjFunction()
//    {
//        //function.Clear();
//        List<GameObject> wrongObjectList = new();
//        foreach (var obj in targetObject)
//        {
//            //IFunctionalObject _obj = obj.GetComponent<IFunctionalObject>();
//            if (_obj == null)
//            {
//                Debug.LogWarning($"{obj.name} 에는 IFunctionalObject 가 없다.");
//                wrongObjectList.Add(obj);
//            }
//            else
//            {
//                function.Add(_obj);
//            }
//        }

//        foreach (var obj in wrongObjectList)
//        {
//            targetObject.Remove(obj);
//        }
//    }
//    public void OnTriggerEnter(Collider other)
//    {
//        if (!isActive) return;
//        if (other.gameObject.CompareTag("PlayerCollider"))
//        {
//            for (int i = 0; i < function.Count; ++i)
//            {
//                function[i].Function(true);
//            }
//        }
//    }

//    public void OnTriggerExit(Collider other)
//    {
//        if (!isActive) return;
//        if (other.gameObject.CompareTag("PlayerCollider"))
//        {
//            for (int i = 0; i < function.Count; ++i)
//            {
//                function[i].Function(false);
//            }
//        }
//    }

//    protected override void InitOnPlay()
//    {
//        isActive = true;
//        InitBuffer();
//    }

//    protected override void InitOnRewind()
//    {
//        isActive = false;
//    }

//    protected override void Rewind(float seconds)
//    {

//        if (trackPositionRotation)
//            RestorePositionAndRotation(seconds);
//        if (trackVelocity)
//            RestoreVelocity(seconds);
//        if (trackAnimator)
//            RestoreAnimator(seconds);
//        if (trackParticles)
//            RestoreParticles(seconds);
//        if (trackAudio)
//            RestoreAudio(seconds);
//    }

//    protected override void Track()
//    {
//        if (trackPositionRotation)
//            TrackPositionAndRotation();
//        if (trackVelocity)
//            TrackVelocity();
//        if (trackAnimator)
//            TrackAnimator();
//        if (trackParticles)
//            TrackParticles();
//        if (trackAudio)
//            TrackAudio();
//    }
//    private void Start()
//    {
//        InitializeParticles(particleSettings);
//        CheckWrongObjFunction();
//    }

//#if UNITY_EDITOR
//    private void OnDrawGizmosSelected()
//    {
//        Handles.color = Color.red;
//        for (int i = 0; i < targetObject.Count; ++i)
//        {
//            Vector3 from, to;
//            GUIStyle style = new();
//            style.fontStyle = FontStyle.Bold;
//            style.alignment = TextAnchor.LowerCenter;
//            style.normal.textColor = Color.white;

//            from = transform.position;
//            to = targetObject[i].transform.position;

//            Handles.DrawLine(from, to, 5);
//            Handles.Label(Vector3.Lerp(from, to, 0.5f), $"Button : {buttonName}", style);
//        }
//    }
//#endif
}
