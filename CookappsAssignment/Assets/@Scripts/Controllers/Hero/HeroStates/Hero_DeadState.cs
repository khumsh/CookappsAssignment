using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using DG.Tweening;

public class Hero_DeadState : IState
{
    private Hero hero;

    public string StateName => ECreatureState.Dead.ToString();

    public Hero_DeadState(Hero hero)
    {
        this.hero = hero;
    }

    public void Enter()
    {
        hero.col2D.isTrigger = true;

        hero.CreatureState = ECreatureState.Dead;
        hero.PlayAnimation(StateName);
    }

    public void Update()
    {
        if (hero.StateMachine.stateTimer > HERO_REVIVAL_TIME && !Managers.Game.IsGameOver)
        {
            hero.ChangeState(ECreatureState.Idle);
        }
    }

    public void Exit()
    {
        hero.col2D.isTrigger = false;
        hero.Target = null;

        hero.CreatureState = ECreatureState.Idle;
        hero.HeroStats.Hp = hero.HeroStats.MaxHp.Value;
        hero.transform.DOLocalJump(Vector3.up * 0.5f, 1, 1, 0.5f);
        Managers.Resource.Instantiate("Effect/Revival", hero.transform);
    }
}
