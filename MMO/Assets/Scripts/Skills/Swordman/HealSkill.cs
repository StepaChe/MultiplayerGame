using UnityEngine;
using UnityEngine.Networking;

public class HealSkill : Skill {

    [SerializeField] int healAmount = 10;
    [SerializeField] ParticleSystem particle;

    protected override void OnCastComplete() {
        if (isServer) unit.myStats.AddHealth(healAmount);
        else particle.Play();
        base.OnCastComplete();
    }
}
