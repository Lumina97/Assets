using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Pool_Base))]
public class PoolingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Pool_Base Pool = (Pool_Base)target;

        DrawDefaultInspector();

        if (Pool)
        {
            Pool.PoolSize = EditorGUILayout.IntField("Poolsize", Pool.PoolSize);
            if (GUILayout.Button("Spawn Pool items"))
            {
                Pool.SpawnPoolItems(true);
            }

            if(GUILayout.Button("Clear Pool"))
            {
                Pool.ClearPreviousPool();
            }
        }
    }
}

[CustomEditor(typeof(MissilePool))]
public class MissilePoolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MissilePool Pool = (MissilePool)target;

        DrawDefaultInspector();

        if (Pool)
        {
            Pool.PoolSize = EditorGUILayout.IntField("Poolsize", Pool.PoolSize);
            if (GUILayout.Button("Spawn Pool items"))
            {
                Pool.SpawnPoolItems(true);
            }

            if (GUILayout.Button("Clear Pool"))
            {
                Pool.ClearPreviousPool();
            }
        }
    }
}

[CustomEditor(typeof(PowerUpPool))]
public class PowerUpPoolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PowerUpPool Pool = (PowerUpPool)target;

        DrawDefaultInspector();

        if (Pool)
        {
            Pool.PoolSize = EditorGUILayout.IntField("Poolsize", Pool.PoolSize);
            if (GUILayout.Button("Spawn Pool items"))
            {
                Pool.SpawnPoolItems(true);
            }

            if (GUILayout.Button("Clear Pool"))
            {
                Pool.ClearPreviousPool();
            }
        }
    }
}