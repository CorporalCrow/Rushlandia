using UnityEngine;

public class SkillScriptableObject : ScriptableObject
{
    public float cooldown = 10f;
    public int damage = 5;
    public int unlockLevel = 1;

    public bool isActivating;

    protected float useTime;

    public virtual SkillScriptableObject ScaleUpForLevel(ScalingScriptableObject scaling, int level)
    {
        SkillScriptableObject scaledSkill = CreateInstance<SkillScriptableObject>();

        ScaleUpBaseValuesForLevel(scaledSkill, scaling, level);

        return scaledSkill;
    }

    protected virtual void ScaleUpBaseValuesForLevel(SkillScriptableObject instance, ScalingScriptableObject scaling, int level)
    {
        instance.name = name;

        instance.cooldown = cooldown;
        instance.damage = damage + Mathf.FloorToInt(scaling.damageCurve.Evaluate(level));
        instance.unlockLevel = unlockLevel;
    }

    public virtual void UseSkill(Enemy enemy, Player player)
    {
        isActivating = true;
    }

    public virtual bool CanUseSkill(Enemy enemy, Player player, int level)
    {
        return level >= unlockLevel;
    }
}