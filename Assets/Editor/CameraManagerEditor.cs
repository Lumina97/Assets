using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraManager))]
public class CameraManagerEditor : Editor
{
    private float _shakeAmount;
    private float _shakeDuration;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CameraManager manager = (CameraManager)target;
        if(manager)
        {
            _shakeAmount = EditorGUILayout.FloatField("Shake amount", _shakeAmount);
            _shakeDuration = EditorGUILayout.FloatField("Shake duration", _shakeDuration);


            if (GUILayout.Button("Test shake."))
                manager.ShakeCamera(_shakeAmount, _shakeDuration);

        }
    }
}
