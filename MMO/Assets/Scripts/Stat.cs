using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Stat
{
    private List<int> modifiers = new List<int>();

    [SerializeField] private int _baseValue;
    // определяем свойство для базового значения
    public int baseValue
    {
        get { return _baseValue; }
        set
        {
            _baseValue = value;
            // при изменении базового значения вызываем ивент
            if (onStatChanged != null) onStatChanged(GetValue());
        }
    }
    public delegate void StatChanged(int value);
    public event StatChanged onStatChanged;

    public int GetValue()
    {
        int finalValue = baseValue;
        modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }

    public void AddModifier(int modifier)
    {
        if (modifier != 0)
        {
            modifiers.Add(modifier);
            if (onStatChanged != null) onStatChanged(GetValue());
        }
    }

    public void RemoveModifier(int modifier)
    {
        if (modifier != 0)
        {
            modifiers.Remove(modifier);
            if (onStatChanged != null) onStatChanged(GetValue());
        }
    }
}
