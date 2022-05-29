using System.Collections;
using UnityEngine;

public abstract class SO_SpawnBehavior : ScriptableObject
{
    public abstract IEnumerator SpawnBehavior(EnemyManager enemymanager);

    public abstract Vector2 FindSpawnPosition();
}