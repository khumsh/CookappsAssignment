using System;
using static Define;

public class StatModifier : IComparable<StatModifier>
{
    public float Value { get; private set; }
    public readonly EStatModType Type;
    public readonly int Order;
    public readonly object Source;

    public event Action<StatModifier> OnValueChanged;

    public StatModifier(float value, EStatModType type, int order, object source)
    {
        Value = value;
        Type = type;
        Order = order;
        Source = source;
    }

    public StatModifier(float value, EStatModType type) : this(value, type, (int)type, null) { }

    public StatModifier(float value, EStatModType type, int order) : this(value, type, order, null) { }

    public StatModifier(float value, EStatModType type, object source) : this(value, type, (int)type, source) { }

    public void UpdateValue(float newValue)
    {
        if (Value != newValue) 
        {
            Value = newValue;
            OnValueChanged?.Invoke(this);
        }
    }

    public int CompareTo(StatModifier other)
    {
        if (Order == other.Order)
            return 0;

        return (Order < other.Order) ? -1 : 1;
    }
}
