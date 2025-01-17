﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Map;

namespace Map
{
    [CustomEditor(typeof(MapGeneratorV2))]
    public class MapEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            MapGeneratorV2 generator = target as MapGeneratorV2;

            if ( GUILayout.Button("SaveMesh") )
            {
                generator.SaveMesh();
            }

            // if ( GUILayout.Button("BuildMesh") )
            // {
            //     generator.BuildMesh();
            // }
        }

    }
}
