using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    int _level = 1, _statPoints;
    float _exp, _nextLevelExp = 100;

    UserData data;

    public void Load(UserData data)
    {
        this.data = data;
        if (data.level > 0) _level = data.level;
        _statPoints = data.statPoints;
        _exp = data.exp;
        if (data.nextLevelExp > 0) _nextLevelExp = data.nextLevelExp;
    }

    public void AddExp(float addExp)
    {
        data.exp = _exp += addExp;

        while (_exp >= _nextLevelExp)
        {
            data.exp = _exp -= _nextLevelExp;
            LevelUP();
        }

        if (_manager != null)
        {
            _manager.exp = _exp;
            _manager.level = _level;
            _manager.nextLevelExp = _nextLevelExp;
            _manager.statPoints = _statPoints;
        }
    }

    private void LevelUP()
    {
        data.level = ++_level;
        data.nextLevelExp = _nextLevelExp += 50f;
        data.statPoints = _statPoints += 3;
    }

    public bool RemoveStatPoint()
    {
        if (_statPoints > 0)
        {
            data.statPoints = --_statPoints;
            if (_manager != null) _manager.statPoints = _statPoints;
            return true;
        }
        return false;
    }

    // менеджер характеристик
    StatsManager _manager;

    // обновление синхронизируемых полей при установке менеджера
    public StatsManager manager
    {
        set
        {
            _manager = value;
            _manager.exp = _exp;
            _manager.nextLevelExp = _nextLevelExp;
            _manager.level = _level;
            _manager.statPoints = _statPoints;
        }
    }
}
