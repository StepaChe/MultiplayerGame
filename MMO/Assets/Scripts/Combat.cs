using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(UnitStats))]
public class Combat : NetworkBehaviour
{
    public float attackDistance = 0f;
    [SerializeField] float attackSpeed = 1f;
    float attackCooldown = 0f;
    UnitStats myStats;

    public delegate void CombatDelegate();
    [SyncEvent] public event CombatDelegate EventOnAttack;

    void Start()
    {
        myStats = GetComponent<UnitStats>();
    }

    private void Update()
    {
        if (attackCooldown > 0) attackCooldown -= Time.deltaTime;
    }

    public bool Attack(UnitStats targetStats)
    {
        if (attackCooldown <= 0)
        {
            EventOnAttack();
            targetStats.TakeDamage(myStats.damage.GetValue());
            attackCooldown = 1f / attackSpeed;
            return true;
        }
        return false;
    }

    public void GetDamage(UnitStats targetStats)
    {
        targetStats.TakeDamage(myStats.damage.GetValue());
    }
}
