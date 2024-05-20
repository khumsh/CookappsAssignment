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

        // Ÿ�� ��ġ�� ��
        Vector3 SearchPoint = Owner.transform.position;
        float SearchRange = (skillData.SkillType == ESkillType.Default) ? Owner.HeroData.DefaultAtkRange : Owner.HeroData.SkillRange;

        // ���� ��ų
        if (skillData.SkillTargetSearchType == ESkillTargetSearchType.Single)
        {
            // ���� �� Ÿ�� ��ġ
            Creature target = Owner.GetTargetInRange(SearchPoint, SearchRange, skillData.TargetType, skillData.TargetSearchResult, skillData.canSelf);

            // Ÿ�ٿ��� ����Ʈ ����
            foreach (ESkillEffectType effectType in skillData.SkillEffectTypes)
            {
                ApplyEffect(target, effectType);
            }
        }
        // ����(����) ��ų
        else if (skillData.SkillTargetSearchType == ESkillTargetSearchType.Range)
        {
            // ���� �� Ÿ�� ���� ��ġ
            Creature[] targets = Owner.GetTargetsInRange(SearchPoint, SearchRange, skillData.TargetType, skillData.canSelf);

            if (targets != null && targets.Length > 0) 
            {
                // Ÿ�� ���ο��� ����Ʈ ����
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

        // ��ų ��ٿ� ���·� ����
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
        if (!target.IsValid()) return;

        float value = Owner.HeroStats.Atk.Value * skillData.AtkRate;

        switch (skillEffectType)
        {
            case ESkillEffectType.DealDamage:
                target.OnDamaged(value, Owner, skill);
                break;
            case ESkillEffectType.Heal:
                target.OnDamaged(-value, Owner, skill);
                break;
            case ESkillEffectType.Stun:
                target.StateMachine.SetState(ECreatureState.Stun.ToString());
                Managers.Object.ShowFloatingText("Stun!", target.Position + Vector3.up, Color.white);
                break;  
        }
    }
}
