using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Hero_IdleState : IState
{
    private Hero hero;
    private HeroStats heroStats => hero.HeroStats;
    public string StateName => ECreatureState.Idle.ToString();

    public Hero_IdleState(Hero hero)
    {
        this.hero = hero;
    }

    public void Enter()
    {
        Debug.Log($"{hero.name} : Enter [{StateName}] State");

        hero.PlayAnimation(StateName);
    }

    public void Update()
    {
        if (hero.Target == null)
        {
            Creature target = hero.GetTargetInRange(hero.transform.position, 5, ETargetType.Monster);
            if (target != null)
            {
                hero.Target = target;
                hero.ChangeState(ECreatureState.Move);
            }
        }
        else
        {
            bool canUseSpecialSkill = GetDistSqrToTarget() <= heroStats.SkillRange.Value * heroStats.SkillRange.Value
                && hero.SkillSystem.SpecialSkill.IsReady();
            bool canUseDefaultSkill = GetDistSqrToTarget() <= heroStats.DefaultAtkRange.Value * heroStats.DefaultAtkRange.Value
                && hero.SkillSystem.DefaultSkill.IsReady();

            if (canUseSpecialSkill || canUseDefaultSkill)
            {
                hero.ChangeState(ECreatureState.Atk);
            }
        }
        
    }

    public void Exit()
    {
        
    }

    // target과의 거리
    private float GetDistSqrToTarget()
    {
        return (hero.Target.transform.position - hero.transform.position).sqrMagnitude;
    }
}
