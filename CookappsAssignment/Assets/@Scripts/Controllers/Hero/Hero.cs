using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Hero : Creature
{
    public HeroData HeroData { get; private set; }
    public HeroStats HeroStats { get; private set; }
    public SkillSystem SkillSystem { get; private set; }

    public EMoveState MoveState { get; private set; }
    public EAtkState AtkState { get; private set; }

    public override void SetInfo(int templateId)
    {
        base.SetInfo(templateId);

        HeroData = Managers.Data.HeroDic[templateId];

        SkillSystem = gameObject.GetOrAddComponent<SkillSystem>();
        SkillSystem.AddSkill(HeroData.DefaultSkillId, ESkillSlot.Default);
        SkillSystem.AddSkill(HeroData.SpecialSkillId, ESkillSlot.Special);

        StatsInit();
        StateMachineInit();
    }

    protected void StatsInit()
    {
        HeroStats = new HeroStats();
        HeroStats.MaxHp = new Stat(HeroData.MaxHp);
        HeroStats.Hp = HeroStats.MaxHp.Value;
        HeroStats.Atk = new Stat(HeroData.Atk);
        HeroStats.DefaultAtkRange = new Stat(HeroData.DefaultAtkRange);
        HeroStats.DefaultAtkCooltime = new Stat(HeroData.DefaultAtkCooltime);
        HeroStats.SkillRange = new Stat(HeroData.SkillRange);
        HeroStats.SkillCooltime = new Stat(HeroData.SkillCooltime);
        HeroStats.MoveSpeed = new Stat(HeroData.MoveSpeed);
    }

    protected void SkillInit()
    {

    }

    protected override void StateMachineInit()
    {
        base.StateMachineInit();

        StateMachine.AddState(new Hero_IdleState(this));
        StateMachine.AddState(new Hero_MoveState(this));
        StateMachine.AddState(new Hero_AtkState(this));
        StateMachine.AddState(new Hero_DeadState(this));

        ChangeState(ECreatureState.Idle);
    }

    public override void OnDamaged(float dmg, Creature attacker, Skill skill)
    {
        base.OnDamaged(dmg, attacker, skill);

        HeroStats.Hp = Mathf.Clamp(HeroStats.Hp - dmg, 0, HeroStats.MaxHp.Value);
        Debug.Log($"{name}'s HP : {HeroStats.Hp}/{HeroStats.MaxHp}");
    }

    protected override void OnDead()
    {
        base.OnDead();
    }

}