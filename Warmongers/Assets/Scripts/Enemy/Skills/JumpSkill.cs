using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Jump Skill", menuName = "ScriptableObject/Skills/Jump")]
public class JumpSkill : SkillScriptableObject
{
    public float minJumpDistance = 1.5f;
    public float maxJumpDistance = 5f;
    public AnimationCurve heightCurve;
    public float jumpSpeed = 1;

    public override SkillScriptableObject ScaleUpForLevel(ScalingScriptableObject scaling, int level)
    {
        JumpSkill instance = CreateInstance<JumpSkill>();

        ScaleUpBaseValuesForLevel(instance, scaling, level);
        instance.minJumpDistance = minJumpDistance;
        instance.maxJumpDistance = maxJumpDistance;
        instance.heightCurve = heightCurve;
        instance.jumpSpeed = jumpSpeed;

        return instance;
    }

    public override bool CanUseSkill(Enemy enemy, Player player, int level)
    {
        if (base.CanUseSkill(enemy, player, level))
        {
            float distance = Vector3.Distance(enemy.transform.position, player.transform.position);

            return !isActivating
                && useTime + cooldown < Time.time
                && distance >= minJumpDistance
                && distance <= maxJumpDistance;
        }

        return false;
    }

    public override void UseSkill(Enemy enemy, Player player)
    {
        base.UseSkill(enemy, player);
        enemy.StartCoroutine(Jump(enemy, player));
    }

    private IEnumerator Jump(Enemy enemy, Player player)
    {
        enemy.agent.enabled = false;
        enemy.movement.enabled = false;
        enemy.movement.state = EnemyState.UsingAbility;
        if (enemy.movement.defaultState == EnemyState.Idle && enemy.agent.speed == enemy.movement.defaultSpeed * 2)
        {
            enemy.agent.speed *= enemy.movement.idleMovespeedMultiplier;
        }

        Vector3 playerPosition = player.transform.position;
        Vector3 startingPosition = enemy.transform.position;
        //enemy.Animator.SetTrigger(EnemyMovement.jump);

        for (float time = 0; time < 1; time += Time.deltaTime * jumpSpeed)
        {
            enemy.transform.position = Vector3.Lerp(startingPosition, playerPosition, time) + Vector3.up * heightCurve.Evaluate(time);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, Quaternion.LookRotation(player.transform.position - enemy.transform.position),time);

            yield return null;
        }
        //enemy.Animator.SetTrigger(EnemyMovement.landed);

        useTime = Time.time;

        enemy.enabled = true;
        enemy.movement.enabled = true;
        enemy.agent.enabled = true;

        if (NavMesh.SamplePosition(playerPosition, out NavMeshHit hit, 1f, enemy.agent.areaMask))
        {
            enemy.agent.Warp(hit.position);
            enemy.movement.state = EnemyState.Chase;

        }

        isActivating = false;
    }
}