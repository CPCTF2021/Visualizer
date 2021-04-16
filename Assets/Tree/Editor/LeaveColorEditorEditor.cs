using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TreeScripts;

namespace TreeScripts
{
    [CustomEditor(typeof(TreeScripts.LeaveColorEditor))]
    public class LeaveColorEditorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            TreeScripts.LeaveColorEditor inst = target as TreeScripts.LeaveColorEditor;

            if (GUILayout.Button("SetColor")){
                inst.SetColor();
            }
        }
    }
}
