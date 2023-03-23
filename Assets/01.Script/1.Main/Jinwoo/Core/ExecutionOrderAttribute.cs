using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif
using UnityEngine;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
sealed class ExecutionOrderAttribute : Attribute
{

    public readonly int ExecutionOrder = 0;

    public ExecutionOrderAttribute(int executionOrder)
    {
        ExecutionOrder = executionOrder;
    }

#if UNITY_EDITOR
    private const string PB_TITLE = "실행 순서 업데이트";
    private const string PB_MESSAGE = "아아앙 기모띠!! 킹아";
    private const string ERR_MESSAGE = "{0}에 대한 실행 순서를 찾아 설정할 수 없음";

    [InitializeOnLoadMethod] //awake호출 이전에 실행 (참고로 메소드 에트리뷰트는 정적 메소드에만 거시기함)
    private static void Execute()
    {
        var type = typeof(ExecutionOrderAttribute);
        var assembly = type.Assembly; 
        var types = assembly.GetTypes();//ExecutionOrderAttribute이거 붙은 넘들 타입가져옴
        var scripts = new Dictionary<MonoScript, ExecutionOrderAttribute>();

        var progress = 0f;
        var step = 1f / types.Length;

        foreach (var item in types)
        {
            var attributes = item.GetCustomAttributes(type, false);
            if (attributes.Length != 1) continue;
            var attribute = attributes[0] as ExecutionOrderAttribute;

            var asset = "";
            var guids = AssetDatabase.FindAssets(string.Format("{0} t:script", item.Name));

            if (guids.Length > 1)
            {
                foreach (var guid in guids)
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    var filename = Path.GetFileNameWithoutExtension(assetPath);
                    if (filename == item.Name)
                    {
                        asset = guid;
                        break;
                    }
                }
            }
            else if (guids.Length == 1)
            {
                asset = guids[0];
            }
            else
            {
                Debug.LogErrorFormat(ERR_MESSAGE, item.Name);
                return;
            }

            var script = AssetDatabase.LoadAssetAtPath<MonoScript>(AssetDatabase.GUIDToAssetPath(asset));
            scripts.Add(script, attribute);
        }

        var changed = false;
        foreach (var item in scripts)
        {
            if (MonoImporter.GetExecutionOrder(item.Key) != item.Value.ExecutionOrder)
            {
                changed = true;
                break;
            }
        }

        if (changed)
        {
            foreach (var item in scripts)
            {
                var cancelled = EditorUtility.DisplayCancelableProgressBar(PB_TITLE, PB_MESSAGE, progress);
                progress += step;

                if (MonoImporter.GetExecutionOrder(item.Key) != item.Value.ExecutionOrder)
                {
                    MonoImporter.SetExecutionOrder(item.Key, item.Value.ExecutionOrder);
                }

                if (cancelled) break;
            }
        }

        EditorUtility.ClearProgressBar();
    }
#endif
}