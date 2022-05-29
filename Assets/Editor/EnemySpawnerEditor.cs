using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyManager))]
public class EnemySpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EnemyManager spawner = (EnemyManager)target;
        DrawDefaultInspector();
        if (spawner)

        {
            if (GUILayout.Button("Spawn single enemy"))
                spawner.SpawnSingleEnemy();

        }
    }
}
