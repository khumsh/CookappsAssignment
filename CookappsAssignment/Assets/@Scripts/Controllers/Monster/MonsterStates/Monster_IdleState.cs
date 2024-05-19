using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

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

        monster.Target = null;

        // ��ó�� ĳ���Ͱ� ������ Ÿ������ ����
        Creature target = monster.GetTargetInRange(monster.transform.position, DETECT_RANGE_MONSTER, ETargetType.Hero);
        if (target.IsValid())
        {
            monster.Target = target;
            // ��� Move ���·� ����
            monster.ChangeState(ECreatureState.Move);
        }
    }

    public void Update()
    {
        // Patrol
        {
            // ���� Ȯ���� ���� ���·� ����
            float rand = Random.value;
            if (rand <= patrolProb)
            {
                monster.ChangeState(ECreatureState.Move);
            }
        }

        // Set Target
        {
            // ��ó�� ĳ���Ͱ� ������ Ÿ������ ����
            Creature target = monster.GetTargetInRange(monster.Position, DETECT_RANGE_MONSTER, ETargetType.Hero);
            if (target.IsValid())
            {
                monster.Target = target;
                // ��� Move ���·� ����
                monster.ChangeState(ECreatureState.Move);
            }
        }
    }

    public void Exit()
    {

    }
}
