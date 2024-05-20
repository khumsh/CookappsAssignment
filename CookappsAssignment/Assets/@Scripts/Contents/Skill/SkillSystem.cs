using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class SkillSystem : MonoBehaviour
{
    public Creature Owner { get; private set; }

    public List<Skill> SkillList { get; private set; } = new List<Skill>();
    public Skill DefaultSkill { get; private set; }
    public Skill SpecialSkill { get; private set; }

    private void Awake()
    {
        Owner = GetComponent<Creature>();
    }

    public void AddSkill(int skillId, ESkillType skillType)
    {
        if (skillId == 0)
            return;

        if (Owner == null)
            Owner = GetComponent<Hero>();

        Skill skill = new Skill();

        skill.SetInfo(skillId, Owner);
        SkillList.Add(skill);

        switch (skillType)
        {
            case ESkillType.Default:
                DefaultSkill = skill;
                break;
            case ESkillType.Special:
                SpecialSkill = skill;
                break;
        }
    }

    private void Update()
    {
        if (DefaultSkill != null)
            DefaultSkill.Update();
        if (SpecialSkill != null)
            SpecialSkill.Update();
    }

    public bool IsUseableSkill()
    {
        return (SpecialSkill != null && SpecialSkill.IsReady()) 
            || (DefaultSkill != null && DefaultSkill.IsReady());
    }

    // 사용가능한 스킬 get
    public Skill GetUseableSkill()
    {
        if (SpecialSkill != null && SpecialSkill.IsReady())
            return SpecialSkill;
        else if (DefaultSkill != null && DefaultSkill.IsReady())
            return DefaultSkill;
        else
            return null;
    }

    public void Clear()
    {
        SkillList.Clear();
    }
}
