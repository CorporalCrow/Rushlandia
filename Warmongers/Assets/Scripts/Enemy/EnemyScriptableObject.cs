using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObject/Enemy Configuration")]
public class EnemyScriptableObject : ScriptableObject
{
    public Enemy prefab;
    public AttackScriptableObject attackConfiguration;
    public SkillScriptableObject[] skills;

    // Enemy Stats
    public int health = 100;

    // Movement State
    public EnemyState defaultState;
    public float idleLocationRadius = 4f;
    public float idleMovespeedMultiplier = 0.5f;
    [Range(2, 10)]
    public int waypoints = 4;
    public float lineOfSightRange = 10f;
    public float fieldOfView = 360f;

    // NavMeshAgent Configuration
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

    public EnemyScriptableObject  ScaleUpForLevel(ScalingScriptableObject scaling, int level)
    {
        EnemyScriptableObject scaledUpEnemy = CreateInstance<EnemyScriptableObject>();

        scaledUpEnemy.name = name;
        scaledUpEnemy.prefab = prefab;

        scaledUpEnemy.attackConfiguration = attackConfiguration.ScaleUpForLevel(scaling, level);

        scaledUpEnemy.skills = new SkillScriptableObject[skills.Length];
        for (int i = 0; i < skills.Length; i++)
        {
            scaledUpEnemy.skills[i] = skills[i].ScaleUpForLevel(scaling, level);
        }

        scaledUpEnemy.health = Mathf.FloorToInt(health * scaling.healthCurve.Evaluate(level));

        scaledUpEnemy.defaultState = defaultState;
        scaledUpEnemy.idleLocationRadius = idleLocationRadius;
        scaledUpEnemy.idleMovespeedMultiplier = idleMovespeedMultiplier;
        scaledUpEnemy.waypoints = waypoints;
        scaledUpEnemy.lineOfSightRange = lineOfSightRange;
        scaledUpEnemy.fieldOfView = fieldOfView;

        scaledUpEnemy.aIUpdateInterval = aIUpdateInterval;
        scaledUpEnemy.acceleration = acceleration;
        scaledUpEnemy.angularSpeed = angularSpeed;

        scaledUpEnemy.areaMask = areaMask;
        scaledUpEnemy.avoidancePriority = avoidancePriority;

        scaledUpEnemy.baseOffset = baseOffset;
        scaledUpEnemy.height = height;
        scaledUpEnemy.obstacleAvoidanceType = obstacleAvoidanceType;
        scaledUpEnemy.radius = radius;
        scaledUpEnemy.speed = speed * scaling.speedCurve.Evaluate(level);
        scaledUpEnemy.stoppingDistance = stoppingDistance;

        return scaledUpEnemy;
    }

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
        enemy.movement.defaultSpeed = speed;
        enemy.agent.stoppingDistance = stoppingDistance;

        enemy.movement.updateRate = aIUpdateInterval;
        enemy.movement.defaultState = defaultState;
        enemy.movement.idleMovespeedMultiplier = idleMovespeedMultiplier;
        enemy.movement.idleLocationRadius = idleLocationRadius;
        enemy.movement.waypoints = new Vector3[waypoints];
        enemy.movement.lineOfSightChecker.fieldOfView = fieldOfView;
        enemy.movement.lineOfSightChecker._collider.radius = lineOfSightRange;
        enemy.movement.lineOfSightChecker.lineOfSightLayers = attackConfiguration.lineOfSightLayers;

        enemy.health = health;

        attackConfiguration.SetupEnemy(enemy);
    }
}
