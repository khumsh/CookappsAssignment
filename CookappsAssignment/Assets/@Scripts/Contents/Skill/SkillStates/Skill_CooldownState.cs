using Data;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Skill_CooldownState : IState
{
    private Skill skill;

    private float cooldownTime;
    public string StateName => ESkillState.Cooldown.ToString();

    public Skill_CooldownState(Skill skill)
    {
        this.skill = skill;
    }

    public void Enter()
    {
        if (skill.Owner.IsPlayer)
        {
            HeroData ownerData = (skill.Owner as Hero).HeroData;
            cooldownTime = (skill.SkillData.SkillType == ESkillType.Default) ?
                        ownerData.DefaultAtkCooltime : ownerData.SkillCooltime;
        }
        else
        {
            MonsterData ownerData = (skill.Owner as Monster).MonsterData;
            cooldownTime = 1 / ownerData.AtkCountPerSecond;
        }

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
