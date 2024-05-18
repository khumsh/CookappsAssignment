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
        Debug.Log($"{hero.name} : Enter [{StateName}] State");

        hero.PlayAnimation(StateName);
    }

    public void Update()
    {
        if (hero.Target != null)
        {
            MoveToTarget();

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

    private void MoveToTarget()
    {
        Vector3 dir = hero.Target.transform.position - hero.transform.position;
        float moveDist = Mathf.Min(dir.magnitude, heroStats.MoveSpeed.Value * Time.deltaTime);
        hero.transform.position += dir.normalized * moveDist;
    }

    // target���� �Ÿ�
    private float GetDistSqrToTarget()
    {
        return (hero.Target.transform.position - hero.transform.position).sqrMagnitude;
    }
}
