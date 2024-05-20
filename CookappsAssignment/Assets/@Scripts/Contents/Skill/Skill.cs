using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Skill
{
    public Creature Owner { get; private set; }
    public SkillData SkillData { get; private set; }

    public EntityStateMachine StateMachine { get; protected set; }

    
    public virtual void SetInfo(int templateId, Creature owner)
    {
        Owner = owner;
        SkillData = Managers.Data.SkillDic[templateId];

        StateMachineInit();
    }

    public void UseSkill()
    {
        // Use »óÅÂ·Î
        ChangeState(ESkillState.Use);
    }

    public void Update()
    {
        if (StateMachine != null)
            StateMachine.Update();
    }

    protected virtual void StateMachineInit()
    {
        StateMachine = new EntityStateMachine();

        StateMachine.AddState(new Skill_ReadyState(this));
        StateMachine.AddState(new Skill_UseState(this));
        StateMachine.AddState(new Skill_CooldownState(this));

        ChangeState(ESkillState.Ready);
    }

    public void ChangeState(ESkillState nextState)
    {
        StateMachine.SetState(nextState.ToString());
    }

    public bool IsReady()
    {
        return StateMachine.GetCurrentStateName() == ESkillState.Ready.ToString();
    }
}
