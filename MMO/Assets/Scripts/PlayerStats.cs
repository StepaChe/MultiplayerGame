using UnityEngine;

public class PlayerStats : UnitStats
{    
    StatsManager _manager;

    UserData data;

    public void Load(UserData data)
    {
        this.data = data;
        curHealth = data.curHealth;
        if (data.statDamage > 0) damage.baseValue = data.statDamage;
        if (data.statArmor > 0) armor.baseValue = data.statArmor;
        if (data.statMoveSpeed > 0) moveSpeed.baseValue = data.statMoveSpeed;
    }

    public override int curHealth
    {
        get { return base.curHealth; }
        protected set
        {
            base.curHealth = value;
            data.curHealth = curHealth;
        }
    }

    public StatsManager manager
    {
        set
        {
            _manager = value;
            _manager.damage = damage.GetValue();
            _manager.armor = armor.GetValue();
            _manager.moveSpeed = moveSpeed.GetValue();
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        damage.onStatChanged += DamageChanged;
        armor.onStatChanged += ArmorChanged;
        moveSpeed.onStatChanged += MoveSpeedChanged;
    }

    private void DamageChanged(int value)
    {
        if (damage.baseValue != data.statDamage)
            data.statDamage = damage.baseValue;
        if (_manager != null) _manager.damage = value;
    }

    private void ArmorChanged(int value)
    {
        if (armor.baseValue != data.statArmor)
            data.statArmor = armor.baseValue;
        if (_manager != null) _manager.armor = value;
    }

    private void MoveSpeedChanged(int value)
    {
        if (moveSpeed.baseValue != data.statMoveSpeed)
            data.statMoveSpeed = moveSpeed.baseValue;
        if (_manager != null) _manager.moveSpeed = value;
    }
}
