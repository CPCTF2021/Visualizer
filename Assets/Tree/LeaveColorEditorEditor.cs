using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TreeScripts
{
    [CustomEditor(typeof(LeaveColorEditor))]
    public class LeaveColorEditorEditor : Editor
    {
        public void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            LeaveColorEditor inst = target as LeaveColorEditor;

            if (GUILayout.Button("SetColor")){
                inst.SetColor();
            }
        }
    }
}
