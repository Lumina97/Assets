using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public float MoveSpeed = 1;
    public float LookRange = 40f;
    public float LookSphereCastRadius = 1f;

    public float ScoreAddedOnDeath = 50f;
}