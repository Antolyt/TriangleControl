using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TriangleComplexUpdate))]
public class TriangleComplexUpdateEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TriangleComplexUpdate updater = (TriangleComplexUpdate)target;
        if (GUILayout.Button("Remove Deactivated Elements"))
        {
            updater.RemoveDeactivatedElements();
        }
        if (GUILayout.Button("Update FunctionalityImages"))
        {
            updater.UpdateFunctionalityImage();
        }
        if (GUILayout.Button("Update Particle System"))
        {
            updater.UpdateParticleSystem();
        }
    }
}
