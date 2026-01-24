using System;
using System.Collections.Generic;
using UnityEngine;

public class Modifier
{
    // public virtual void Apply()
    // {
    //     Vars.Instance.modifiers.AddModifier(this);
    // }     
    // public virtual void Cancel()
    // {
    //     Vars.Instance.modifiers.RemoveModifier(this);
    // }     

    public bool IsInflucing() => true;
}

public interface IBonus
{
    public float Bonus {get;set;}        
}
public interface IMultiplier
{
    public float Multiplier {get;set;}        
}

public class DetailQualityModifier : Modifier, IBonus, IFormattable
{
    public float Bonus { get; set; }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        return $"Detail quality: {(Bonus < 0 ? "-" : "+")}{(int)(Mathf.Abs(Bonus) * 100)}\n";
    }
}
public class MaterialPriceModifier : Modifier, IBonus, IMultiplier, IFormattable
{
    public float Bonus { get; set; }
    public float Multiplier { get; set; }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        string str = string.Empty;
        if (Bonus != 0)
        {
            str += "Material Price: ";
            str += Bonus > 0 ? "+" : "-";
            str += Mathf.Abs(Bonus);
            str += "\n";
        }
        if (Multiplier != 0)
        {
            str += "Material Price: ";
            str += Multiplier > 0 ? "+" : "-";
            str += Mathf.Abs(Multiplier);
            str += "%\n";
        }
        return str;
    }
}
public class EffeciencyGrowModifier : Modifier, IBonus, IMultiplier, IFormattable
{
    public float Bonus { get; set; }
    public float Multiplier { get; set; }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        string str = string.Empty;
        if (Bonus != 0)
        {
            str += $"Effeciency Grow: {(Bonus > 0 ? "+" : "-")}{(int)(Mathf.Abs(Bonus) * 100)}\n";
        }
        if (Multiplier != 0)
        {
            str += $"Effeciency Grow: {(Multiplier > 0 ? "+" : "-")}{(int)(Mathf.Abs(Multiplier) * 100)}\n";
        }
        return str;
    }
}
public class MaxEffeciencyModifier : Modifier, IBonus, IMultiplier, IFormattable
{
    public float Bonus { get; set; }
    public float Multiplier { get; set; }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        string str = string.Empty;
        if (Bonus != 0)
        {
            str += $"Max Effeciency: {(Bonus > 0 ? "+" : "-")}{(int)(Mathf.Abs(Bonus) * 100)}\n";
        }
        if (Multiplier != 0)
        {
            str += $"Max Effeciency: {(Multiplier > 0 ? "+" : "-")}{(int)(Mathf.Abs(Multiplier) * 100)}%\n";
        }
        return str;
    }
}

public class BuildSpeedModifier : Modifier, IBonus, IFormattable
{
    public float Bonus { get; set; }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        string str = string.Empty;
        if (Bonus != 0)
        {
            str += $"Build Speed: {(Bonus > 0 ? "+" : "-")}{(int)(Mathf.Abs(Bonus) * 100)}\n";
        }
        return str;
    }
}

public class ResearchSpeedModifier : Modifier, IBonus, IFormattable
{
    public float Bonus { get; set; }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        string str = string.Empty;
        if (Bonus != 0)
        {
            str += $"Research Speed: {(Bonus > 0 ? "+" : "-")}{(int)(Mathf.Abs(Bonus) * 100)}\n";
        }
        return str;
    }
}

public class IncomeTaxModifier : Modifier, IBonus, IFormattable
{
    public float Bonus { get; set; }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        string str = string.Empty;
        if (Bonus != 0)
        {
            str += $"Income Tax: {(Bonus > 0 ? "+" : "-")}{(int)(Mathf.Abs(Bonus) * 100)}\n";
        }
        return str;
    }
}

public class InfluenceGrowModifier : Modifier, IBonus, IMultiplier, IFormattable
{
    public float Bonus { get; set; }
    public float Multiplier { get; set; }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        string str = string.Empty;
        if (Bonus != 0)
        {
            str += $"Influence: {(Bonus > 0 ? "+" : "-")}{(int)(Mathf.Abs(Bonus) * 100)}\n";
        }
        if (Multiplier != 0)
        {
            str += $"Influence: {(Multiplier > 0 ? "+" : "-")}{(int)(Mathf.Abs(Multiplier) * 100)}%\n";
        }
        return str;
    }
}

public class ModifiersSystem
{
    public Dictionary<Type, List<Modifier>> modifiers = new();

    public void Init()
    {
        Restart();
    }

    public void Restart()
    {
        modifiers.Clear();
    }

    public List<Modifier> GetModifiers(Type modifierType)
    {
        return modifiers.TryGetValue(modifierType, out var l) ? l : new();
    }
    public List<Modifier> GetModifiers<T>() => GetModifiers(typeof(T));

    public void AddModifier(Modifier modifier)
    {
        List<Modifier> l;
        if (!modifiers.TryGetValue(modifier.GetType(), out l))
        {
            l = new();
            modifiers[modifier.GetType()] = l;
        }
        l.Add(modifier);        
    }
    public void RemoveModifier(Modifier m)
    {
        if (modifiers.TryGetValue(m.GetType(), out var l))
        {
            l.Remove(m);
        }
    }

    public float GetBonus<T>() where T : IBonus
    {
        float total = 0;
        foreach (var m in GetModifiers<T>())
        {
            if (m is IBonus bm)
            {
                total += bm.Bonus; 
            }
        }
        return total;
    }
    public float GetMultiplier<T>() where T : IMultiplier
    {
        float total = 0;
        foreach (var m in GetModifiers<T>())
        {
            if (m is IMultiplier bm)
            {
                total += bm.Multiplier; 
            }
        }
        return total;
    }
}