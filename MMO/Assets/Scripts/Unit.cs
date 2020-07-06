using UnityEngine;
using UnityEngine.Networking;

public class Unit : Interactable
{
    [SerializeField] UnitMotor _motor;
    [SerializeField] UnitStats _stats;
    public UnitStats myStats { get { return _stats; } }
    public UnitMotor motor { get { return _motor; } }

    protected Interactable _focus;
    public Interactable focus { get { return _focus; } }

    public UnitSkills unitSkills;

    protected bool isDead;
    protected float interactDistance;

    public delegate void UnitDelegate();
    [SyncEvent] public event UnitDelegate EventOnDamage;
    [SyncEvent] public event UnitDelegate EventOnDie;
    [SyncEvent] public event UnitDelegate EventOnRevive;

    public override void OnStartServer()
    {
        motor.SetMoveSpeed(myStats.moveSpeed.GetValue());
        myStats.moveSpeed.onStatChanged += motor.SetMoveSpeed;
    }

    void Update ()
    {
        OnUpdate();
    }

    public override float GetInteractDistance(GameObject user)
    {
        Combat combat = user.GetComponent<Combat>();
        return base.GetInteractDistance(user) + (combat != null ? combat.attackDistance : 0f);
    }

    protected virtual void OnAliveUpdate() { }
    protected virtual void OnDeadUpdate() { }

    protected void OnUpdate()
    {
        if (isServer)
        {
            if (!isDead)
            {
                if (myStats.curHealth == 0) Die();
                else OnAliveUpdate();
            }
            else
            {
                OnDeadUpdate();
            }
        }
    }

    public override bool Interact(GameObject user)
    {
        Combat combat = user.GetComponent<Combat>();
        if (combat != null)
        {
            if (combat.Attack(myStats))
            {
                // вместо вызова ивента используем новый метод, передавая ему
                // атакующий объект
                DamageWithCombat(user);
            }
            return true;
        }
        return base.Interact(user);
    }    

    public virtual void SetFocus(Interactable newFocus)
    {
        if (newFocus != focus)
        {
            _focus = newFocus;
            interactDistance = focus.GetInteractDistance(gameObject);
            motor.FollowTarget(newFocus, interactDistance);
        }
    }

    public virtual void RemoveFocus()
    {
        _focus = null;
        motor.StopFollowingTarget();
    }

    [ClientCallback]
    protected virtual void Die()
    {
        isDead = true;
        GetComponent<BoxCollider>().enabled = false;
        EventOnDie();

        if (isServer)
        {
            RemoveFocus();
            hasInteract = false;
            //motor.MoveToPoint(transform.position);
            RpcDie();
        }
    }

    [ClientRpc]
    void RpcDie()
    {
        if (!isServer) Die();
    }

    [ClientCallback]
    protected virtual void Revive()
    {
        isDead = false;
        GetComponent<BoxCollider>().enabled = true;
        EventOnRevive();

        if (isServer)
        {
            hasInteract = true;
            myStats.SetHealthRate(1);
            RpcRevive();
        }
    }

    [ClientRpc]
    void RpcRevive()
    {
        if (!isServer) Revive();
    }

    // метод, срабатывающий при получении урона в бою
    protected virtual void DamageWithCombat(GameObject user)
    {
        EventOnDamage();
    }

    public void UseSkill(int skillNum)
    {
        if (!isDead  && skillNum < unitSkills.Count)
        {
            unitSkills[skillNum].Use(this);
        }
    }

    public void TakeDamage(GameObject user, int damage)
    {
        myStats.TakeDamage(damage);
        DamageWithCombat(user);
    }
}
