using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Skill_ReadyState : IState
{
    private Skill skill;

    public string StateName => ESkillState.Ready.ToString();

    public Skill_ReadyState(Skill skill)
    {
        this.skill = skill;
    }

    public void Enter()
    {
        Debug.Log($"{skill.SkillData.SkillName} : Enter [{StateName}] State");
    }

    public void Update()
    {
        
    }

    public void Exit()
    {

    }
}
