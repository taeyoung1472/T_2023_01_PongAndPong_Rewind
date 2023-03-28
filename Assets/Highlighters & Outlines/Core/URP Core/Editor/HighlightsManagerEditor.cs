using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

namespace Highlighters
{
#if UNITY_EDITOR
    [CustomEditor(typeof(HighlightsManager))]
    public class HighlightsManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }
    }
#endif
}