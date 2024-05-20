using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Monster_MoveState : IState
{
    private Monster monster;
    private EMoveState moveState = EMoveState.Patrol;

    private Vector3 destPos;

    public string StateName => ECreatureState.Move.ToString();

    public Monster_MoveState(Monster monster)
    { 
        this.monster = monster; 
    }

    public void Enter()
    {
        monster.CreatureState = ECreatureState.Move;
        monster.PlayAnimation(StateName);

        moveState = (monster.Target.IsValid()) ? EMoveState.Chase : EMoveState.Patrol;

        if (moveState == EMoveState.Patrol)
        {
            destPos = Util.GenerateRandomPositionOnCircle(monster.Position, 2.5f);
        }

        Debug.Log($"{monster.name} : Enter [{StateName}] State\n" +
            $"MoveState : {moveState}");
    }

    public void Update()
    {
        // 순찰
        if (moveState == EMoveState.Patrol)
        {
            MoveToPosition(destPos);

            float distSqr = (destPos - monster.Position).sqrMagnitude;
            if (distSqr < 0.1f)
            {
                monster.ChangeState(ECreatureState.Idle);
            }
        }
        // 추적
        else if (moveState == EMoveState.Chase && monster.Target.IsValid())
        {
            Vector3 dir = monster.Target.Position - monster.Position;
            float distToTargetSqr = dir.sqrMagnitude;
            float atkRangeSqr = monster.MonsterData.AtkRange * monster.MonsterData.AtkRange;
            float detectRangeSqr = DETECT_RANGE_MONSTER * DETECT_RANGE_MONSTER;

            if (distToTargetSqr < atkRangeSqr)
            {
                // 공격 범위 이내라면 공격 상태로 전이
                Debug.Log("Monster Move -> Attack");
                monster.ChangeState(ECreatureState.Atk);
            }
            else if (distToTargetSqr < detectRangeSqr)
            {
                // 공격 범위 밖이고 감지 범위 이내라면 추적
                MoveToTarget();
            }
            else
            {
                // 감지 범위 밖이라면 추적 종료
                monster.Target = null;
                monster.ChangeState(ECreatureState.Idle);
            }
        }
        else
        {
            monster.Target = null;
            monster.ChangeState(ECreatureState.Idle);
        }
    }

    public void Exit()
    {

    }

    private void MoveToTarget()
    {
        Vector3 dir = monster.Target.Position - monster.Position;
        float moveDist = Mathf.Min(dir.magnitude, monster.MonsterStats.MoveSpeed.Value * Time.deltaTime);
        monster.transform.position += dir.normalized * moveDist;

        monster.Flip();
    }

    private void MoveToPosition(Vector3 pos)
    {
        Vector3 dir = pos - monster.Position;
        float moveDist = Mathf.Min(dir.magnitude, monster.MonsterStats.MoveSpeed.Value * Time.deltaTime);
        monster.transform.position += dir.normalized * moveDist;

        bool isRightDir = dir.x > 0;
        monster.Flip(isRightDir);
    }
}
