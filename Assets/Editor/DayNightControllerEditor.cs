using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DayNightController))]
public class DayNightControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Bake Env Lighting"))
            FindObjectOfType<EnvBaker>().Bake();
    }
}
