using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Monster_IdleState : IState
{
    private Monster monster;

    public string StateName => ECreatureState.Idle.ToString();

    public Monster_IdleState(Monster monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        monster.PlayAnimation(StateName);
    }

    public void Exit()
    {

    }

    public void Update()
    {

    }
}
