using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ButtonTest : MonoBehaviour
{
    [Header("[Button]")]
    private bool isActive = false;
    [SerializeField] private string buttonName;
    [SerializeField] private List<GameObject> targetObject;
    List<IFunctionalObject> function = new();
    private void OnValidate()
    {
        function.Clear();
        List<GameObject> wrongObjectList = new();
        foreach (var obj in targetObject)
        {
            IFunctionalObject _obj = obj.GetComponent<IFunctionalObject>();
            if (_obj == null)
            {
                Debug.LogWarning($"{obj.name} 에는 IFunctionalObject 가 없다.");
                wrongObjectList.Add(obj);
            }
            else
            {
                function.Add(_obj);
            }
        }

        foreach (var obj in wrongObjectList)
        {
            targetObject.Remove(obj);
        }
    }
    public void InitOnPlay()
    {
        isActive = true;
    }

    public void InitOnRewind()
    {
        isActive = false;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;
        if (other.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < function.Count; ++i)
            {
                function[i].Function(true);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (!isActive) return;
        if (other.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < function.Count; ++i)
            {
                function[i].Function(false);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        for (int i = 0; i < targetObject.Count; ++i)
        {
            Vector3 from, to;
            GUIStyle style = new();
            style.fontStyle = FontStyle.Bold;
            style.alignment = TextAnchor.LowerCenter;
            style.normal.textColor = Color.white;

            from = transform.position;
            to = targetObject[i].transform.position;

            Handles.DrawLine(from, to, 5);
            Handles.Label(Vector3.Lerp(from, to, 0.5f), $"Button : {buttonName}", style);
        }
    }
#endif
}
