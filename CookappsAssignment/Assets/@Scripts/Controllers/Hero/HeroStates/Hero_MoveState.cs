using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Hero_MoveState : IState
{
    private Hero hero;
    private EMoveState moveState => hero.MoveState;

    public string StateName => ECreatureState.Move.ToString();

    public Hero_MoveState(Hero hero)
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
