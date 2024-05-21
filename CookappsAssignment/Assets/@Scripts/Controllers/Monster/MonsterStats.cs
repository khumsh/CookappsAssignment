using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStats
{
    public MonsterStats(Monster owner)
    {
        Owner = owner;
    }

    private Monster Owner;

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
    public Stat AtkRange { get; set; }
    public Stat AtkCountPerSecond { get; set; }

    public Stat MoveSpeed { get; set; }
}
