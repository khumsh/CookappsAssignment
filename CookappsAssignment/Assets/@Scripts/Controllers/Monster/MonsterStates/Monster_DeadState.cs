using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using DG.Tweening;

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
        monster.CreatureState = ECreatureState.Dead;
        monster.PlayAnimation(StateName);

        foreach(var sp in monster.spriteRenderers)
        {
            sp.DOFade(0, 1).OnComplete(() =>
            {
                sp.color = Color.white;
                Managers.Object.Despawn(monster);
            });
        }
        
    }

    public void Update()
    {
        
    }

    public void Exit()
    {

    }
}
