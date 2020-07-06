using UnityEngine;
using UnityEngine.Networking;

public class FrontWarpSkill : Skill {

    [SerializeField] float warpDistance = 7f;

    protected override void OnUse() {
        if (isServer) {
            unit.motor.StopFollowingTarget();
        }
        base.OnUse();
    }

    protected override void OnCastComplete() {
        if (isServer) {
            unit.transform.Translate(Vector3.forward * warpDistance);
            unit.motor.StopFollowingTarget();
        }
        base.OnCastComplete();
    }
}
