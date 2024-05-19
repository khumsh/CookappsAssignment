using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Skill_UseState : IState
{
    private Skill skill;
    private Hero Owner => skill.Owner;
    private SkillData skillData => skill.SkillData;

    public string StateName => ESkillState.Use.ToString();

    public Skill_UseState(Skill skill)
    {
        this.skill = skill;
    }

    public void Enter()
    {
        Debug.Log($"{skill.SkillData.SkillName} : Enter [{StateName}] State");

        // 타겟 서치용 값
        Vector3 SearchPoint = Owner.transform.position;
        float SearchRange = (skillData.SkillType == ESkillType.Default) ? Owner.HeroData.DefaultAtkRange : Owner.HeroData.SkillRange;

        // 단일 스킬
        if (skillData.SkillTargetSearchType == ESkillTargetSearchType.Single)
        {
            // 범위 내 타겟 서치
            Creature target = Owner.GetTargetInRange(SearchPoint, SearchRange, skillData.TargetType);

            // 타겟에게 이펙트 적용
            foreach (ESkillEffectType effectType in skillData.SkillEffectTypes)
            {
                ApplyEffect(target, effectType);
            }
        }
        // 다중(범위) 스킬
        else if (skillData.SkillTargetSearchType == ESkillTargetSearchType.Range)
        {
            // 범위 내 타겟 전부 서치
            Creature[] targets = Owner.GetTargetsInRange(SearchPoint, SearchRange, skillData.TargetType);

            if (targets != null && targets.Length > 0) 
            {
                // 타겟 전부에게 이펙트 적용
                foreach (Creature t in targets)
                {
                    foreach (ESkillEffectType effectType in skillData.SkillEffectTypes)
                    {
                        ApplyEffect(t, effectType);
                    }
                }
            }
            
            
        }

        if (skill.SkillData.SkillType == ESkillType.Special)
            Managers.Object.ShowFloatingText("Special Skill!", Owner.Position, Color.yellow);

        // 스킬 쿨다운 상태로 전이
        skill.ChangeState(ESkillState.Cooldown);
    }

    public void Update()
    {

    }

    public void Exit()
    {

    }

    private void ApplyEffect(Creature target, ESkillEffectType skillEffectType)
    {
        float value = Owner.HeroStats.Atk.Value * skillData.AtkRate;

        switch (skillEffectType)
        {
            case ESkillEffectType.DealDamage:
                if (target.IsValid())
                    target.OnDamaged(value, Owner, skill);
                break;
            case ESkillEffectType.Heal:
                if (target.IsValid())
                    target.OnDamaged(-value, Owner, skill);
                break;
            case ESkillEffectType.Stun:
                if (target.IsValid())
                {
                    target.StateMachine.SetState(ECreatureState.Stun.ToString());
                    Managers.Object.ShowFloatingText("Stun!", target.Position + Vector3.up, Color.white);
                }
                break;  
        }
    }
}
