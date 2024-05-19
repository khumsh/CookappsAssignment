using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Skill_CooldownState : IState
{
    private Skill skill;
    private HeroData ownerData => skill.Owner.HeroData;

    private float cooldownTime;
    public string StateName => ESkillState.Cooldown.ToString();

    public Skill_CooldownState(Skill skill)
    {
        this.skill = skill;
    }

    public void Enter()
    {
        cooldownTime = (skill.SkillData.SkillType == ESkillType.Default) ?
            ownerData.DefaultAtkCooltime : ownerData.SkillCooltime;

        Debug.Log($"{skill.SkillData.SkillName} : Enter [{StateName}] State\n" +
            $"CooldownTime : {cooldownTime}");
    }

    public void Update()
    {

        if (skill.StateMachine.stateTimer >= cooldownTime)
        {
            skill.ChangeState(ESkillState.Ready);
        }
    }

    public void Exit()
    {

    }
}
