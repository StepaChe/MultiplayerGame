using UnityEngine;

[RequireComponent(typeof(UnitMotor), typeof(PlayerStats))]
public class Character : Unit
{
    
    [SerializeField] float reviveDelay = 5f;
    [SerializeField] GameObject gfx;
    
    public Vector3 startPosition;
    public Player player;
    public GameObject _rightHolder;  // ссылки на пустышки в руках
    public GameObject _lefttHolder; //
    float reviveTime;

    new public PlayerStats stats { get { return myStats as PlayerStats; } }

    void Start()
    {
        startPosition = Vector3.zero;
        reviveTime = reviveDelay;

        if (stats.curHealth == 0)
        {
            transform.position = startPosition;
            if (isServer)
            {
                stats.SetHealthRate(1);
                motor.MoveToPoint(startPosition);
            }
        }
    }

    void Update()
    {
        OnUpdate();
    }

    protected override void OnAliveUpdate()
    {
        base.OnAliveUpdate();
        if (focus != null)
        {
            if (!focus.hasInteract)
            {
                RemoveFocus();
            }
            else
            {
                float distance = Vector3.Distance(focus.interactionTransform.position, transform.position);
                if (distance <= interactDistance)
                {
                    //focus.Interact(gameObject);
                    if (!focus.Interact(gameObject)) RemoveFocus();
                }
            }
        }
    }

    protected override void OnDeadUpdate()
    {
        base.OnDeadUpdate();
        if (reviveTime > 0)
        {
            reviveTime -= Time.deltaTime;
        }
        else
        {
            reviveTime = reviveDelay;
            Revive();
        }
    }

    protected override void Die()
    {
        base.Die();
        //gfx.SetActive(false);
    }

    protected override void Revive()
    {
        base.Revive();
        transform.position = startPosition;
        //gfx.SetActive(true);
        if (isServer)
        {
            transform.position = startPosition;
            //motor.MoveToPoint(startPosition);
        }
    }

    public void SetMovePoint(Vector3 point)
    {
        if (!isDead)
        {
            RemoveFocus();
            motor.MoveToPoint(point);
        }
    }

    public void SetNewFocus(Interactable newFocus)
    {
        if (!isDead)
        {
            if (newFocus.hasInteract) SetFocus(newFocus);
        }
    }
    
}
