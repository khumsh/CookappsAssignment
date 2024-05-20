using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Monster_StunState : IState
{
    private Monster monster;
    private float stunningTime = 1f;

    public string StateName => ECreatureState.Stun.ToString();

    public Monster_StunState(Monster monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        Debug.Log($"{monster.name} : Enter [{StateName}] State");

        monster.CreatureState = ECreatureState.Stun;
        monster.PlayAnimation("Idle");

        monster.Target = null;
    }

    public void Update()
    {
        if (monster.StateMachine.stateTimer >= stunningTime)
            monster.ChangeState(ECreatureState.Idle);
    }

    public void Exit()
    {

    }
}
