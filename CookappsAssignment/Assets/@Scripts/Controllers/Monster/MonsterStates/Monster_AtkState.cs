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
        if (!monster.Target.IsValid())
        {
            monster.Target = null;
            monster.ChangeState(ECreatureState.Idle);
            return;
        }

        // Atk 애니메이션 속도 설정
        float animationSpeed = monster.MonsterStats.AtkCountPerSecond.Value;
        monster.animator.SetFloat("AtkSpeedMult", animationSpeed);

        monster.CreatureState = ECreatureState.Atk;
        monster.PlayAnimation(StateName);

        monster.Flip();
    }

    public void Update()
    {
        if (!monster.Target.IsValid())
            monster.ChangeState(ECreatureState.Idle);

        if (monster.IsAnimationDone(StateName))
            monster.ChangeState(ECreatureState.Idle);
    }

    public void Exit()
    {
        // 애니메이션 속도를 기본값으로 재설정.
        monster.animator.SetFloat("AtkSpeedMult", 1);
    }

}
