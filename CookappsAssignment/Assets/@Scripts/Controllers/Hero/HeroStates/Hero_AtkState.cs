using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Hero_AtkState : IState
{
    private Hero hero;

    public string StateName => ECreatureState.Atk.ToString();

    public Hero_AtkState(Hero hero)
    {
        this.hero = hero;
    }

    public void Enter()
    {
        hero.CreatureState = ECreatureState.Atk;
        hero.PlayAnimation(StateName);

        Skill skill = hero.SkillSystem.GetUseableSkill();
        if (skill != null) 
            skill.UseSkill();
        else
            hero.ChangeState(ECreatureState.Idle);

        hero.Flip();

        hero.col2D.isTrigger = true;
    }

    public void Update()
    {
        if (!hero.Target.IsValid())
            hero.ChangeState(ECreatureState.Idle);

        if (hero.IsAnimationDone(StateName))
            hero.ChangeState(ECreatureState.Idle);
    }

    public void Exit()
    {
        hero.col2D.isTrigger = false;
    }
}
