using UnityEngine;

public class AuraStrikeSkill : Skill {

    [SerializeField] int damage;
    [SerializeField] float radius;
    [SerializeField] LayerMask enemyMask;
    [SerializeField] ParticleSystem auraEffect;

    protected override void OnUse() {
        if (isServer) {
            unit.motor.StopFollowingTarget();
        }
        base.OnUse();
    }

    protected override void OnCastComplete() {
        if (isServer) {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius, enemyMask);
            for (int i = 0; i < colliders.Length; i++) {
                Unit enemy = colliders[i].GetComponent<Unit>();
                if (enemy != null && enemy.hasInteract) enemy.TakeDamage(unit.gameObject, damage);
            }
        } else {
            auraEffect.Play();
        }
        base.OnCastComplete();
    }

    protected void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
