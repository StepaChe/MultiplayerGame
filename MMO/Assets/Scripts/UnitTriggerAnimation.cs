using UnityEngine;

public class UnitTriggerAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Unit unit;
    [SerializeField] Combat combat;

    private void Start()
    {
        unit.EventOnDamage += Damage;
        unit.EventOnDie += Die;
        unit.EventOnRevive += Revive;
        combat.EventOnAttack += Attack;
    }

    private void Damage()
    {
        animator.SetTrigger("Damage");
    }

    private void Die()
    {
        animator.SetTrigger("Die");
    }

    private void Revive()
    {
        animator.ResetTrigger("Damage");
        animator.ResetTrigger("Attack");
        animator.SetTrigger("Revive");
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
    }
}
