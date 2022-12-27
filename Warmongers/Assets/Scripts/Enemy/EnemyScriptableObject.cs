using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Enemy Configuration", menuName = "ScriptableObject/Enemy Configuration")]
public class EnemyScriptableObject : ScriptableObject
{
    public Enemy prefab;
    public AttackScriptableObject attackConfiguration;

    // Enemy Stats
    public int health = 100;

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

    public void SetUpEnemy(Enemy enemy)
    {
        enemy.agent.acceleration = acceleration;
        enemy.agent.angularSpeed = angularSpeed;
        enemy.agent.areaMask = areaMask;
        enemy.agent.avoidancePriority = avoidancePriority;
        enemy.agent.baseOffset = baseOffset;
        enemy.agent.height = height;
        enemy.agent.obstacleAvoidanceType = obstacleAvoidanceType;
        enemy.agent.radius = radius;
        enemy.agent.speed = speed;
        enemy.agent.stoppingDistance = stoppingDistance;

        enemy.movement.updateRate = aIUpdateInterval;

        enemy.health = health;

        attackConfiguration.SetupEnemy(enemy);
    }
}
