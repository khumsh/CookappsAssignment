using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Hero_IdleState : IState
{
    private Hero hero;
    private HeroStats heroStats => hero.HeroStats;
    
    private float detectRange = 30; // 과제 파일에 없어서 임의로 지정

    public string StateName => ECreatureState.Idle.ToString();

    public Hero_IdleState(Hero hero)
    {
        this.hero = hero;
    }

    public void Enter()
    {
        Debug.Log($"{hero.name} : Enter [{StateName}] State");

        hero.CreatureState = ECreatureState.Idle;
        hero.PlayAnimation(StateName);
    }

    public void Update()
    {
        if (hero.Target.IsValid())
        {
            bool isInSkillRange = GetDistSqrToTarget() <= heroStats.SkillRange.Value * heroStats.SkillRange.Value;
            bool isInAtkRange = GetDistSqrToTarget() <= heroStats.DefaultAtkRange.Value * heroStats.DefaultAtkRange.Value;

            if (!isInSkillRange && !isInAtkRange)
            {
                hero.ChangeState(ECreatureState.Move);
                return;
            }   

            bool canUseSpecialSkill = isInSkillRange && hero.SkillSystem.SpecialSkill.IsReady();
            bool canUseDefaultSkill = isInAtkRange && hero.SkillSystem.DefaultSkill.IsReady();

            if (canUseSpecialSkill || canUseDefaultSkill)
            {
                hero.ChangeState(ECreatureState.Atk);
                return;
            }
            else if (!(isInSkillRange && isInAtkRange))
            {
                hero.ChangeState(ECreatureState.Move);
                return;
            }
        }
        else // Search Target
        {
            Creature target = hero.GetTargetInRange(hero.Position, detectRange, ETargetType.Monster);
            if (target.IsValid())
            {
                hero.Target = target;
                hero.ChangeState(ECreatureState.Move);
            }
        }
    }

    public void Exit()
    {
        
    }

    // target과의 거리
    private float GetDistSqrToTarget()
    {
        return (hero.Target.Position - hero.Position).sqrMagnitude;
    }
}
