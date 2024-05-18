using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

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
        hero.PlayAnimation(StateName);
    }

    void IState.Update()
    {
        
    }

    public void Exit()
    {
        
    }
}
