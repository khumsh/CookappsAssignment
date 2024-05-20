using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroStats
{
    public HeroStats(Hero owner)
    {
        Owner = owner;
    }

    private Hero Owner;

    public Stat MaxHp { get; set; }
    private float _hp;
    public float Hp 
    {
        get { return _hp; }
        set
        {
            _hp = Mathf.Clamp(value, 0, MaxHp.Value);
            Owner?.OnHpChanged();
        }
    }
    public Stat Atk { get; set; }
    public Stat DefaultAtkRange { get; set; }
    public Stat DefaultAtkCooltime { get; set; }
    public Stat SkillRange { get; set; }
    public Stat SkillCooltime { get; set; }

    public Stat MoveSpeed { get; set; }

    public int Level { get; set; }
    public Stat MaxExp { get; set; }
    public float Exp { get; set; }
    
}
