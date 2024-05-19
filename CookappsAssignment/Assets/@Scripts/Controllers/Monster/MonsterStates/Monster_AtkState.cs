using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Monster_AtkState : IState
{
    private Monster monster;
    private Coroutine attackCoroutine;
    public string StateName => ECreatureState.Atk.ToString();

    public Monster_AtkState(Monster monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        // Atk 애니메이션 속도 설정
        float animationSpeed = monster.MonsterStats.AtkCountPerSecond.Value;
        monster.animator.SetFloat("AtkSpeedMult", animationSpeed);

        monster.CreatureState = ECreatureState.Atk;
        monster.PlayAnimation(StateName);

        StartAttackCoroutine();
    }

    public void Update()
    {
        if (!monster.Target.IsValid())
        {
            monster.ChangeState(ECreatureState.Idle);
        }
    }

    public void Exit()
    {
        StopAttackCoroutine();

        // 애니메이션 속도를 기본값으로 재설정.
        monster.animator.SetFloat("AtkSpeedMult", 1);
    }

    private void StartAttackCoroutine()
    {
        if (attackCoroutine == null)
        {
            attackCoroutine = monster.StartCoroutine(AttackRoutine());
        }
    }

    private void StopAttackCoroutine()
    {
        if (attackCoroutine != null)
        {
            monster.StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }

    private IEnumerator AttackRoutine()
    {
        float attackInterval = 1f / monster.MonsterStats.AtkCountPerSecond.Value; // Attacks per second
        var wait = new WaitForSeconds(attackInterval); // Cache

        while (true)
        {
            if (monster.Target.IsValid())
            {
                monster.Flip();
                monster.Target.OnDamaged(monster.MonsterStats.Atk.Value, monster, null);
            }
            else
            {
                monster.ChangeState(ECreatureState.Idle);
            }

            yield return wait;
        }
    }
}
