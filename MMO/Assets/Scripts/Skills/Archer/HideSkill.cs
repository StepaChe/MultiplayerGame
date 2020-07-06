using UnityEngine;

public class HideSkill : Skill {

    [SerializeField] ParticleSystem hideEffect;

    protected override void OnUse() {
        if (isServer) {
            unit.RemoveFocus();
            unit.hasInteract = false;
        } else hideEffect.Play();
        base.OnUse();
    }

    protected override void OnCastComplete() {
        if (isServer) {
            unit.hasInteract = true;
        } else hideEffect.Stop();
        base.OnCastComplete();
    }
}
