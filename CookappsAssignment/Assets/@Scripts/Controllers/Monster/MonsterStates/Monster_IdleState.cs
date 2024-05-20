using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using Random = UnityEngine.Random;

public class Monster_IdleState : IState
{
    private Monster monster;

    private float patrolProb = 0.003f;

    public string StateName => ECreatureState.Idle.ToString();

    public Monster_IdleState(Monster monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        monster.CreatureState = ECreatureState.Idle;
        monster.PlayAnimation(StateName);
    }

    public void Update()
    {
        if (monster.Target.IsValid())
        {
            bool isInAtkRange = GetDistSqrToTarget() <= monster.MonsterStats.AtkRange.Value * monster.MonsterStats.AtkRange.Value;

            if (!isInAtkRange) // 공격 범위 밖이면
            {
                // 즉시 Move 상태로 전이
                monster.ChangeState(ECreatureState.Move);
                return;
            }

            bool canUseDefaultSkill = isInAtkRange && monster.SkillSystem.DefaultSkill.IsReady();

            if (canUseDefaultSkill) // 공격 가능 상태면
            {
                // 즉시 Atk 상태로 전이
                Debug.Log("Monster Idle -> Attack");
                monster.ChangeState(ECreatureState.Atk);
                return;
            }
        }
        else
        {
            // Search Target
            {
                // 근처에 캐릭터가 있으면 타겟으로 설정
                Creature target = monster.GetTargetInRange(monster.Position, DETECT_RANGE_MONSTER, ETargetType.Hero);
                if (target.IsValid())
                {
                    monster.Target = target;
                    monster.ChangeState(ECreatureState.Move);
                    return;
                }
            }

            // Patrol
            {
                // 일정 확률로 순찰 상태로 전이
                float rand = Random.value;
                if (rand <= patrolProb)
                {
                    monster.ChangeState(ECreatureState.Move);
                    return;
                }
            }
        }

        
    }

    public void Exit()
    {

    }

    // target과의 거리
    private float GetDistSqrToTarget()
    {
        return (monster.Target.Position - monster.Position).sqrMagnitude;
    }
}
