using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Hero_AtkState : IState
{
    private Hero hero;
    private EAtkState atkState => hero.AtkState;

    public string StateName => ECreatureState.Atk.ToString();

    public Hero_AtkState(Hero hero)
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
