using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Enemy Configuration", menuName = "ScriptableObject/Enemy Configuration")]
public class EnemyScriptableObject : ScriptableObject
{
    // Enemy Stats
    public int health = 100;
    public float attackDelay = 1f;
    public int damage = 5;
    public float attackRadius = 5f;
    public bool isRanged = false;

    //NavMeshAgent Configuration
    public float aIUpdateInterval = 0.1f;

    public float acceleration = 8;
    public float angularSpeed = 120;
    // -1 means everything
    public int areaMask = -1;
    public int avoidancePriority = 50;
    public float baseOffset = 0.5f;
    public float height = 1f;
    public ObstacleAvoidanceType obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
    public float radius = 0.5f;
    public float speed = 3f;
    public float stoppingDistance = 1f;

}
