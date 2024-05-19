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
        if (hero.StateMachine.stateTimer > 5 && !Managers.Game.IsGameOver)
        {
            hero.SetInfo(hero.HeroData.DataId);
            hero.ChangeState(ECreatureState.Idle);
            hero.transform.DOLocalJump(Vector3.up * 0.5f, 1, 1, 0.5f);
            Managers.Resource.Instantiate("Effect/Revival", hero.transform);
        }
    }

    public void Exit()
    {
        hero.col2D.isTrigger = false;
    }
}
