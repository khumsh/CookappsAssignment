using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStats
{
    public Stat MaxHp { get; set; }
    public float Hp { get; set; }
    public Stat Atk { get; set; }
    public Stat AtkRange { get; set; }
    public Stat AtkCountPerSecond { get; set; }

    public Stat MoveSpeed { get; set; }
}
