using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Monster_AtkState : IState
{
    private Monster monster;

    public string StateName => ECreatureState.Atk.ToString();

    public Monster_AtkState(Monster monster)
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
