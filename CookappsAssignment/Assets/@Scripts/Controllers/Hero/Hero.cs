using Data;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static Define;

public class Hero : Creature
{
    public HeroData HeroData { get; private set; }
    public HeroStats HeroStats { get; private set; }

    public EMoveState MoveState { get; private set; }
    public EAtkState AtkState { get; private set; }

    private HeroUIInfo uiInfo;

    protected override bool Init()
    {
        if (!base.Init())
            return false;

        ObjectType = EObjectType.Hero;

        return true;
    }

    public override void SetInfo(int templateId)
    {
        HeroData = Managers.Data.HeroDic[templateId];

        SkillSystem.AddSkill(HeroData.DefaultSkillId, ESkillType.Default);
        SkillSystem.AddSkill(HeroData.SpecialSkillId, ESkillType.Special);

        StatsInit();
        StateMachineInit();

        base.SetInfo(templateId);

        HeroStats.MaxHp.OnValueChanged += OnHpChanged;

        uiInfo = new HeroUIInfo()
        {
            hero = this,
            level = HeroStats.Level,
            maxHp = (int)HeroStats.MaxHp.Value,
            hp = (int)HeroStats.Hp,
            maxExp = (int)HeroStats.MaxExp.Value,
            exp = (int)HeroStats.Exp,
        };

        TriggerHeroUIInfo();
    }

    protected void StatsInit()
    {
        HeroStats = new HeroStats(this);
        HeroStats.MaxHp = new Stat(HeroData.MaxHp);
        HeroStats.Atk = new Stat(HeroData.Atk);
        HeroStats.DefaultAtkRange = new Stat(HeroData.DefaultAtkRange);
        HeroStats.DefaultAtkCooltime = new Stat(HeroData.DefaultAtkCooltime);
        HeroStats.SkillRange = new Stat(HeroData.SkillRange);
        HeroStats.SkillCooltime = new Stat(HeroData.SkillCooltime);
        HeroStats.MoveSpeed = new Stat(HeroData.MoveSpeed);
        HeroStats.MaxExp = new Stat(MAX_EXP_DEFAULT);

        HeroStats.Hp = HeroStats.MaxHp.Value;
        HeroStats.Exp = 0;
        HeroStats.Level = 1;
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

        StateMachine.AddState(new Hero_StunState(this));

        ChangeState(ECreatureState.Idle);
    }

    public override void OnDamaged(float dmg, Creature attacker, Skill skill)
    {
        base.OnDamaged(dmg, attacker, skill);

        // Apply Damage
        HeroStats.Hp -= dmg;

        if (Mathf.Approximately(HeroStats.Hp, 0))
        {
            OnDead();
        }
    }

    protected override void OnDead()
    {
        base.OnDead();

        // Dead 상태로 전이
        ChangeState(ECreatureState.Dead);

        // GameOver Check
        Managers.Game.CheckGameOver();
    }

    public void OnHpChanged()
    {
        hpBar?.SetHpRatio(HeroStats.Hp / HeroStats.MaxHp.Value);

        TriggerHeroUIInfo();
    }

    public void AddExp(float exp)
    {
        HeroStats.Exp += exp;
        if (HeroStats.Exp >= HeroStats.MaxExp.Value)
        {
            LevelUp();
        }

        TriggerHeroUIInfo();
    }

    public void LevelUp()
    {
        string modSource = "LevelUp";

        HeroStats.Level += 1;
        HeroStats.Exp = 0;

        HeroStats.MaxExp.AddModifier(new StatModifier(MAX_EXP_BONUS_MULT, EStatModType.PercentMult, source: modSource));

        HeroStats.Atk.AddModifier(new StatModifier(STAT_BONUS_MULT, EStatModType.PercentMult, source: modSource));
        HeroStats.MaxHp.AddModifier(new StatModifier(STAT_BONUS_MULT, EStatModType.PercentMult, source: modSource));
        HeroStats.Hp = HeroStats.MaxHp.Value;
        
        Managers.Object.ShowFloatingText("Level Up!", Position + Vector3.up * 2, Color.yellow, 6);
        Managers.Resource.Instantiate("Effect/LevelUp", transform);

        Debug.Log($"{name} <color=yellow>Level Up!</color>\n" +
            $"Level : {HeroStats.Level}, Atk : {HeroStats.Atk.Value}, MaxHp : {HeroStats.MaxHp.Value}\n" +
            $"MaxExp : {HeroStats.MaxExp.Value}");

        TriggerHeroUIInfo();
    }

    public void TriggerHeroUIInfo()
    {
        if (uiInfo.hero == null) return;

        uiInfo.level = HeroStats.Level;
        uiInfo.maxHp = (int)HeroStats.MaxHp.Value;
        uiInfo.hp = (int)HeroStats.Hp;
        uiInfo.maxExp = (int)HeroStats.MaxExp.Value;
        uiInfo.exp = (int)HeroStats.Exp;

        HeroSlotUIEvent.Trigger(uiInfo);
    }
}