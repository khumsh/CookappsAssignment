using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Hero_MoveState : IState
{
    private Hero hero;
    private HeroStats heroStats => hero.HeroStats;

    public string StateName => ECreatureState.Move.ToString();

    public Hero_MoveState(Hero hero)
    {
        this.hero = hero;
    }

    public void Enter()
    {
        //Debug.Log($"{hero.name} : Enter [{StateName}] State");

        hero.CreatureState = ECreatureState.Move;
        hero.PlayAnimation(StateName);
    }

    public void Update()
    {
        if (hero.Target.IsValid())
        {
            bool isInSkillRange = GetDistSqrToTarget() <= heroStats.SkillRange.Value * heroStats.SkillRange.Value;
            bool isInAtkRange = GetDistSqrToTarget() <= heroStats.DefaultAtkRange.Value * heroStats.DefaultAtkRange.Value;

            bool canUseSpecialSkill = isInSkillRange && hero.SkillSystem.SpecialSkill.IsReady();
            bool canUseDefaultSkill = isInAtkRange && hero.SkillSystem.DefaultSkill.IsReady();

            if (canUseSpecialSkill || canUseDefaultSkill)
            {
                hero.ChangeState(ECreatureState.Atk);
                return;
            }
            else
            {
                MoveToTarget();
                return;
            }
        }
        else
        {
            hero.ChangeState(ECreatureState.Idle);
        }
            

        
    }

    public void Exit()
    {
        
    }

    private void MoveToTarget()
    {
        Vector3 dir = hero.Target.Position - hero.Position;
        float moveDist = Mathf.Min(dir.magnitude, heroStats.MoveSpeed.Value * Time.deltaTime);
        hero.transform.position += dir.normalized * moveDist;

        hero.Flip();
    }

    // target과의 거리
    private float GetDistSqrToTarget()
    {
        return (hero.Target.Position - hero.Position).sqrMagnitude;
    }
}
