using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class SkillSystem : MonoBehaviour
{
    public Hero Owner { get; private set; }

    public List<Skill> SkillList { get; private set; } = new List<Skill>();
    public Skill DefaultSkill { get; private set; }
    public Skill SpecialSkill { get; private set; }

    private void Awake()
    {
        Owner = GetComponent<Hero>();
    }

    public void AddSkill(int skillId, ESkillSlot skillSlot)
    {
        if (skillId == 0)
            return;

        //string className = Managers.Data.SkillDic[skillId].ClassName;
        //Skill skill = gameObject.AddComponent(Type.GetType(className)) as Skill;

        Skill skill = new Skill();

        if (skill == null)
            return;

        skill.SetInfo(skillId, Owner, skillSlot);
        SkillList.Add(skill);

        switch (skillSlot)
        {
            case ESkillSlot.Default:
                DefaultSkill = skill;
                break;
            case ESkillSlot.Special:
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
        return SpecialSkill.IsReady() || DefaultSkill.IsReady();
    }

    // 사용가능한 스킬 get
    public Skill GetUseableSkill()
    {
        if (SpecialSkill.IsReady())
            return SpecialSkill;
        else if (DefaultSkill.IsReady())
            return DefaultSkill;
        else
            return null;
    }

    public void Clear()
    {
        SkillList.Clear();
    }
}
