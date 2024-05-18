using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Skill/* : BaseObject*/
{
    public Hero Owner { get; private set; }
    public SkillData SkillData { get; private set; }
    public ESkillSlot SkillSlot { get; private set; }

    public EntityStateMachine StateMachine { get; protected set; }

    //protected override bool Init()
    //{
    //    if (!base.Init())
    //        return false;



    //    return true;
    //}

    public virtual void SetInfo(int templateId, Hero owner, ESkillSlot skillSlot)
    {
        Owner = owner;
        SkillData = Managers.Data.SkillDic[templateId];
        SkillSlot = skillSlot;

        StateMachineInit();
    }

    public void UseSkill()
    {
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
