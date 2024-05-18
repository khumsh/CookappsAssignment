using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Hero : Creature
{
    public EMoveState MoveState { get; private set; }
    public EAtkState AtkState { get; private set; }


    protected override void StateInit()
    {
        base.StateInit();

        StateMachine.AddState(new Hero_IdleState(this));
        StateMachine.AddState(new Hero_MoveState(this));
        StateMachine.AddState(new Hero_AtkState(this));
        StateMachine.AddState(new Hero_DeadState(this));

        ChangeState(ECreatureState.Idle);
    }

    protected override void OnDead()
    {
        base.OnDead();
    }

}