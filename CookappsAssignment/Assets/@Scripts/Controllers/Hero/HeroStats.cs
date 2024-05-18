using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroStats
{
    public Stat MaxHp { get; set; }
    public float Hp { get; set; }
    public Stat Atk { get; set; }
    public Stat DefaultAtkRange { get; set; }
    public Stat DefaultAtkCooltime { get; set; }
    public Stat SkillRange { get; set; }
    public Stat SkillCooltime { get; set; }

    public Stat MoveSpeed { get; set; }
}
