using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitMotor : MonoBehaviour
{
    NavMeshAgent agent;
    Transform target;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (target != null)
        {
            // поворот объекта к цели, если она уже достигнута
            if (agent.velocity.magnitude == 0) FaceTarget();
            // установка нового объекта в фокус
            agent.SetDestination(target.position);
        }
    }

    public void MoveToPoint(Vector3 point)
    {
        agent.SetDestination(point);
    }

    public void FollowTarget(Interactable newTarget, float interactDistance)
    {
        agent.stoppingDistance = interactDistance;
        target = newTarget.interactionTransform;
    }

    public void StopFollowingTarget()
    {
        agent.stoppingDistance = 0f;
        agent.ResetPath();
        target = null;
    }

    private void FaceTarget()
    {
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public void SetMoveSpeed(int speed)
    {
        agent.speed = speed;
    }
}
