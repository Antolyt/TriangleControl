using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TriangleComplexBuilder))]
public class TriangleComplexBuilderEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TriangleComplexBuilder builder = (TriangleComplexBuilder)target;
        if (GUILayout.Button("Create"))
        {
            builder.BuildComplex();
        }

    }
}
