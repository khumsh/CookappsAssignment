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
    public SkillSystem SkillSystem { get; private set; }

    public EMoveState MoveState { get; private set; }
    public EAtkState AtkState { get; private set; }

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

        SkillSystem = gameObject.GetOrAddComponent<SkillSystem>();
        SkillSystem.AddSkill(HeroData.DefaultSkillId, ESkillType.Default);
        SkillSystem.AddSkill(HeroData.SpecialSkillId, ESkillType.Special);

        StatsInit();
        StateMachineInit();

        base.SetInfo(templateId);
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

        StateMachine.AddState(new Hero_StunState(this));

        ChangeState(ECreatureState.Idle);
    }

    public override void OnDamaged(float dmg, Creature attacker, Skill skill)
    {
        base.OnDamaged(dmg, attacker, skill);

        // Apply Damage
        {
            HeroStats.Hp = Mathf.Clamp(HeroStats.Hp - dmg, 0, HeroStats.MaxHp.Value);
            Debug.Log($"{name}'s HP : {HeroStats.Hp}/{HeroStats.MaxHp.Value}");
            if (hpBar.gameObject.IsValid())
                hpBar.SetHpRatio(HeroStats.Hp / HeroStats.MaxHp.Value);
        }

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

        bool gameOver = true;
        foreach(var hero in Managers.Object.Heroes)
        {
            if (hero.IsValid())
            {
                gameOver = false;
                break;
            }
        }

        if (gameOver) 
        {
            Managers.Game.IsGameOver = true;
            Managers.UI.ShowToast("Stage Failed...");
        }
    }

    
}