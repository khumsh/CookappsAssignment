using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Monster_DeadState : IState
{
    private Monster monster;

    public string StateName => ECreatureState.Dead.ToString();

    public Monster_DeadState(Monster monster)
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
