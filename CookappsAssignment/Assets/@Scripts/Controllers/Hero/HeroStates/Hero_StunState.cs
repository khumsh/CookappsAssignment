using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static Define;

public class Hero_StunState : IState
{
    private Hero hero;
    private float stunningTime = 1f;

    public string StateName => ECreatureState.Stun.ToString();

    public Hero_StunState(Hero hero)
    {
        this.hero = hero;
    }

    public void Enter()
    {
        Debug.Log($"{hero.name} : Enter [{StateName}] State");

        hero.CreatureState = ECreatureState.Stun;
        hero.PlayAnimation("Idle");
    }

    public void Update()
    {
        if (hero.StateMachine.stateTimer >= stunningTime)
            hero.ChangeState(ECreatureState.Idle);
    }

    public void Exit()
    {

    }
}
