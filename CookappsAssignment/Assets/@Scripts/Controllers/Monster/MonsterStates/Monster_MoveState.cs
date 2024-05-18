using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Monster_MoveState : IState
{
    private Monster monster;

    public string StateName => ECreatureState.Move.ToString();

    public Monster_MoveState(Monster monster)
    { 
        this.monster = monster; 
    }

    public void Enter()
    {
        monster.PlayAnimation(StateName);
    }

    public void Update()
    {

    }

    public void Exit()
    {

    }
}
