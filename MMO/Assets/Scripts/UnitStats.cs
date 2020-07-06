using UnityEngine;
using UnityEngine.Networking;

public class UnitStats : NetworkBehaviour
{

    public int maxHealth;
    [SyncVar] int _curHealth;

    public Stat damage;
    public Stat armor; 
    public Stat moveSpeed;

    public virtual int curHealth
    {
        get { return _curHealth; }
        protected set { _curHealth = value; }
    }

    public override void OnStartServer()
    {
        _curHealth = maxHealth;
    }

    public void SetHealthRate(float rate)
    {
        curHealth = rate == 0 ? 0 : (int)(maxHealth / rate);
    }

    public virtual void TakeDamage(int damage)
    {
        damage -= armor.GetValue();
        if (damage > 0)
        {
            curHealth -= damage;
            if (curHealth <= 0)
            {
                curHealth = 0;
            }
        }
    }

    public void AddHealth(int amount)
    {
        curHealth += amount;
        if (curHealth > maxHealth) curHealth = maxHealth;
    }
}
