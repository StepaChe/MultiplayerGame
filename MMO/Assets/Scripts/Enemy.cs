using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(UnitMotor), typeof(EnemyStats))]
public class Enemy : Unit {

    [Header("Movement")]
    [SerializeField] float moveRadius = 10f;
    [SerializeField] float minMoveDelay = 4f;
    [SerializeField] float maxMoveDelay = 12f;
    Vector3 startPosition;
    Vector3 curDistanation;
    float changePosTime;

    [Header("Behavior")]
    [SerializeField] bool aggressive;
    [SerializeField] float viewDistance = 5f;
    [SerializeField] float reviveDelay = 5f;
    float reviveTime;

    // опасная дистанция
    [SerializeField] float agroDistance = 5f;

    // награда за убийство
    [SerializeField] float rewardExp;

    // список персонажей, атаковавших монстра
    List<Character> enemies = new List<Character>();

    void Start ()
    {
        startPosition = transform.position;
        changePosTime = Random.Range(minMoveDelay, maxMoveDelay);
        reviveTime = reviveDelay;
    }

    void Update()
    {
        OnUpdate();
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

    protected override void OnAliveUpdate()
    {
        base.OnAliveUpdate();
        if (focus == null)
        {
            Wandering(Time.deltaTime);
            if (aggressive) FindEnemy();
        }
        else
        {
            float distance = Vector3.Distance(focus.interactionTransform.position, transform.position);
            if (distance > viewDistance || !focus.hasInteract)
            {
                RemoveFocus();
            }
            else if (distance <= interactDistance)
            {
                //focus.Interact(gameObject);
                if (!focus.Interact(gameObject)) RemoveFocus();
            }
        }
    }

    protected override void Die()
    {
        base.Die();
        if (isServer)
        {
            // начисление награды
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].player.progress.AddExp(rewardExp / enemies.Count);
            }
            // очищение списка
            enemies.Clear();
        }
    }

    protected override void Revive()
    {
        base.Revive();
        transform.position = startPosition;
        if (isServer)
        {
            motor.MoveToPoint(startPosition);
        }
    }

    void Wandering(float deltaTime)
    {
        changePosTime -= deltaTime;
        if (changePosTime <= 0)
        {
            RandomMove();
            changePosTime = Random.Range(minMoveDelay, maxMoveDelay);
        }
    }

    void RandomMove()
    {
        curDistanation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up) * new Vector3(moveRadius, 0, 0) + startPosition;
        motor.MoveToPoint(curDistanation);
    }

    void FindEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, agroDistance, 1 << LayerMask.NameToLayer("Player"));
        for (int i = 0; i < colliders.Length; i++)
        {
            Interactable interactable = colliders[i].GetComponent<Interactable>();
            if (interactable != null && interactable.hasInteract)
            {
                SetFocus(interactable);
                break;
            }
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }

    protected override void DamageWithCombat(GameObject user)
    {
        base.DamageWithCombat(user);
        Unit enemy = user.GetComponent<Unit>();
        if (enemy != null)
        {
            SetFocus(enemy.GetComponent<Interactable>());
            Character character = enemy as Character;
            if (character != null && !enemies.Contains(character))
                enemies.Add(character);
        }
    } 
}
