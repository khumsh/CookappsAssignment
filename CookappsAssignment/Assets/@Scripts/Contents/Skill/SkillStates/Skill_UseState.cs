using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Skill_UseState : IState
{
    private Skill skill;
    private Hero Owner => skill.Owner;
    private SkillData SkillData => skill.SkillData;

    public string StateName => ESkillState.Use.ToString();

    public Skill_UseState(Skill skill)
    {
        this.skill = skill;
    }

    public void Enter()
    {
        Debug.Log($"{skill.SkillData.SkillName} : Enter [{StateName}] State");

        Vector3 SearchPoint = Owner.transform.position;
        float SearchRange = (skill.SkillSlot == ESkillSlot.Default) ? Owner.HeroData.DefaultAtkRange : Owner.HeroData.SkillRange;

        if (SkillData.SkillTargetSearchType == ESkillTargetSearchType.Single)
        {
            Creature target = Owner.GetTargetInRange(SearchPoint, SearchRange, SkillData.TargetType);

            foreach (ESkillEffectType effectType in SkillData.SkillEffectTypes)
            {
                float value = Owner.HeroStats.Atk.Value * SkillData.AtkRate;

                switch (effectType)
                {
                    case ESkillEffectType.DealDamage:
                        target.OnDamaged(value, Owner, skill);
                        break;
                    case ESkillEffectType.Heal:
                        target.OnDamaged(-value, Owner, skill);
                        break;
                    case ESkillEffectType.Stun:
                        break;
                }
            }
        }
        else if (SkillData.SkillTargetSearchType == ESkillTargetSearchType.Range)
        {
            var targets = Owner.GetTargetsInRange(SearchPoint, SearchRange, SkillData.TargetType);


        }

        skill.ChangeState(ESkillState.Cooldown);
    }

    public void Update()
    {

    }

    public void Exit()
    {

    }
}
