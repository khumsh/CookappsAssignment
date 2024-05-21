using System.Collections;
using Data;
using DG.Tweening;
using UnityEngine;
using static Define;

public class Monster : Creature
{
    public MonsterData MonsterData { get; private set; }
    public MonsterStats MonsterStats { get; private set; }

    public EMonsterType MonsterType { get; private set; }

    protected override bool Init()
    {
        base.Init();
        ObjectType = EObjectType.Monster;
        return true;
    }

    public override void SetInfo(int templateId)
    {
        MonsterData = Managers.Data.MonsterDic[templateId];

        MonsterType = MonsterData.MonsterType;

        SkillSystem.AddSkill(MonsterData.DefaultSkillId, ESkillType.Default);

        StatsInit();
        StateMachineInit();

        base.SetInfo(templateId);
    }

    protected void StatsInit()
    {
        // 스테이지당 10%씩 증가
        float statMult = Mathf.Pow(1.1f, Managers.Game.StageLevel - 1);

        MonsterStats = new MonsterStats(this);
        MonsterStats.MaxHp = new Stat(MonsterData.MaxHp * statMult);
        MonsterStats.Hp = MonsterStats.MaxHp.Value;
        MonsterStats.Atk = new Stat(MonsterData.Atk * statMult);
        MonsterStats.AtkRange = new Stat(MonsterData.AtkRange);
        MonsterStats.AtkCountPerSecond = new Stat(MonsterData.AtkCountPerSecond);
        MonsterStats.MoveSpeed = new Stat(MonsterData.MoveSpeed);

        MonsterStats.MaxHp.OnValueChanged += OnHpChanged;
    }

    protected override void StateMachineInit()
    {
        base.StateMachineInit();

        StateMachine.AddState(new Monster_IdleState(this));
        StateMachine.AddState(new Monster_MoveState(this));
        StateMachine.AddState(new Monster_AtkState(this));
        StateMachine.AddState(new Monster_DeadState(this));

        StateMachine.AddState(new Monster_StunState(this));

        ChangeState(ECreatureState.Idle);
    }

    public override void OnDamaged(float dmg, Creature attacker, Skill skill)
    {
        base.OnDamaged(dmg, attacker, skill);

        // Apply Damage
        {
            MonsterStats.Hp = Mathf.Clamp(MonsterStats.Hp - dmg, 0, MonsterStats.MaxHp.Value);
            Debug.Log($"{name}'s HP : {MonsterStats.Hp}/{MonsterStats.MaxHp.Value}");
        }

        if (Mathf.Approximately(MonsterStats.Hp, 0))
        {
            OnDead();

            Hero hero = attacker as Hero;
            if (hero != null)
            {
                hero.AddExp(MONSTER_KILL_EXP);
            }
        }
    }

    protected override void OnDead()
    {
        base.OnDead();

        // Dead 상태로 전이
        ChangeState(ECreatureState.Dead);

        Managers.Game.EnemyKillCount++;
        Managers.Game.Gold += 5;

        if (MonsterType == EMonsterType.Boss)
        {
            Managers.Game.StageClear();
        }

    }

    
}
