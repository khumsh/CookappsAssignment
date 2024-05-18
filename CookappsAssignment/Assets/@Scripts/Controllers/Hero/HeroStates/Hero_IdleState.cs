using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Hero_IdleState : IState
{
    private Hero hero;

    public string StateName => ECreatureState.Idle.ToString();

    public Hero_IdleState(Hero hero)
    {
        this.hero = hero;
    }

    public void Enter()
    {
        hero.PlayAnimation(StateName);
    }

    public void Update()
    {
        
    }

    public void Exit()
    {
        
    }
}
