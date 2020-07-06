using UnityEngine;
using UnityEngine.AI;

public class UnitAnimation : MonoBehaviour {

    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent agent;

    void FixedUpdate() {
        if (agent.velocity.magnitude == 0) {
            animator.SetBool("Move", false);
        } else {
            animator.SetBool("Move", true);
        }
    }

    //Placeholder functions for Animation events
    void Hit() {
    }

    void FootR() {
    }

    void FootL() {
    }
}
