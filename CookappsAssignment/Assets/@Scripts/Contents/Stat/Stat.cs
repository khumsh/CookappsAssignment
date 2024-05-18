using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Stat
{
    public float BaseValue { get; private set; } // 기본 값 (연산 전 값)

    private bool _isDirty = true; // dirtyFlag. 값에 변화가 생겼는지 여부

    [SerializeField]
    private float _value;
    public virtual float Value
    {
        get
        {
            if (_isDirty) // 값에 변화가 생겼다면
            {
                _value = CalculateFinalValue(); // 다시 계산
                _isDirty = false;
            }
            return _value;
        }

        private set { _value = value; }
    }

    public List<StatModifier> StatModifiers = new List<StatModifier>();

    public Stat()
    {
    }

    public Stat(float baseValue) : this()
    {
        BaseValue = baseValue;
    }

    public virtual void AddModifier(StatModifier modifier)
    {
        _isDirty = true;
        StatModifiers.Add(modifier);
    }

    public virtual bool RemoveModifier(StatModifier modifier)
    {
        if (StatModifiers.Remove(modifier))
        {
            _isDirty = true;
            return true;
        }

        return false;
    }

    public virtual bool ClearModifiersFromSource(object source)
    {
        int numRemovals = StatModifiers.RemoveAll(mod => mod.Source == source);

        if (numRemovals > 0)
        {
            _isDirty = true;
            return true;
        }
        return false;
    }

    private float CalculateFinalValue()
    {
        float finalValue = BaseValue;
        float sumPercentAdd = 0;

        StatModifiers.Sort();

        for (int i = 0; i < StatModifiers.Count; i++)
        {
            StatModifier modifier = StatModifiers[i];

            switch (modifier.Type)
            {
                case EStatModType.Add:
                    finalValue += modifier.Value;
                    break;
                case EStatModType.PercentAdd:
                    sumPercentAdd += modifier.Value;
                    if (i == StatModifiers.Count - 1 || StatModifiers[i + 1].Type != EStatModType.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0;
                    }
                    break;
                case EStatModType.PercentMult:
                    finalValue *= 1 + modifier.Value;
                    break;
            }
        }

        return (float)Math.Round(finalValue, 4);
    }
}
