using System.Collections;
using Data;
using UnityEngine;
using static Define;

public class Monster : Creature
{
    public MonsterData MonsterData { get; private set; }
    public MonsterStats MonsterStats { get; private set; }

    protected override bool Init()
    {
        base.Init();
        ObjectType = EObjectType.Monster;
        return true;
    }

    public override void SetInfo(int templateId)
    {
        base.SetInfo(templateId);
        
        MonsterData = Managers.Data.MonsterDic[templateId];

        StatsInit();
        StateMachineInit();
    }

    protected void StatsInit()
    {
        MonsterStats = new MonsterStats();
        MonsterStats.MaxHp = new Stat(MonsterData.MaxHp);
        MonsterStats.Hp = MonsterStats.MaxHp.Value;
        MonsterStats.Atk = new Stat(MonsterData.Atk);
        MonsterStats.AtkRange = new Stat(MonsterData.AtkRange);
        MonsterStats.AtkCountPerSecond = new Stat(MonsterData.AtkCountPerSecond);
        MonsterStats.MoveSpeed = new Stat(MonsterData.MoveSpeed);
    }

    protected override void StateMachineInit()
    {
        base.StateMachineInit();

        StateMachine.AddState(new Monster_IdleState(this));
        StateMachine.AddState(new Monster_MoveState(this));
        StateMachine.AddState(new Monster_AtkState(this));
        StateMachine.AddState(new Monster_DeadState(this));

        ChangeState(ECreatureState.Idle);
    }

    public override void OnDamaged(float dmg, Creature attacker, Skill skill)
    {
        base.OnDamaged(dmg, attacker, skill);

        MonsterStats.Hp = Mathf.Clamp(MonsterStats.Hp - dmg, 0, MonsterStats.MaxHp.Value);
        Debug.Log($"{name}'s HP : {MonsterStats.Hp}/{MonsterStats.MaxHp}");
    }

    protected override void OnDead()
    {
        base.OnDead();

        
    }
    
}
