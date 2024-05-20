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

            if (!isInAtkRange) // ���� ���� ���̸�
            {
                // ��� Move ���·� ����
                monster.ChangeState(ECreatureState.Move);
                return;
            }

            bool canUseDefaultSkill = isInAtkRange && monster.SkillSystem.DefaultSkill.IsReady();

            if (canUseDefaultSkill) // ���� ���� ���¸�
            {
                // ��� Atk ���·� ����
                Debug.Log("Monster Idle -> Attack");
                monster.ChangeState(ECreatureState.Atk);
                return;
            }
        }
        else
        {
            // Search Target
            {
                // ��ó�� ĳ���Ͱ� ������ Ÿ������ ����
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
                // ���� Ȯ���� ���� ���·� ����
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

    // target���� �Ÿ�
    private float GetDistSqrToTarget()
    {
        return (monster.Target.Position - monster.Position).sqrMagnitude;
    }
}
