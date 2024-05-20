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
        if (!hero.Target.IsValid())
        {
            hero.ChangeState(ECreatureState.Idle);
            return;
        }

        hero.CreatureState = ECreatureState.Atk;
        hero.PlayAnimation(StateName);

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
