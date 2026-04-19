//using UnityEngine;
//using UnityEngine.AI;
//using UnityEngine.InputSystem;

//public class CharacterPathfind : MonoBehaviour
//{
//    public Camera cam;
//    public NavMeshAgent agent;
//    public Animator animator;

//    void Start()
//    {
//        agent.updatePosition = false;
//        agent.updateRotation = false;
//    }

//    void Update()
//    {
//        HandleInput();
//        UpdateAnimation();
//        RotateCharacter();
//    }

//    void HandleInput()
//    {
//        if (Mouse.current.leftButton.wasPressedThisFrame)
//        {
//            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
//            RaycastHit hit;

//            if (Physics.Raycast(ray, out hit))
//            {
//                agent.SetDestination(hit.point);
//            }
//        }
//    }

//    void UpdateAnimation()
//    {
//        float speed = agent.velocity.magnitude / agent.speed;
//        animator.SetFloat("Speed", speed);
//    }

//    void RotateCharacter()
//    {
//        if (agent.velocity.magnitude > 0.1f)
//        {
//            Quaternion rot = Quaternion.LookRotation(agent.velocity);
//            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 10);
//        }
//    }

//    void OnAnimatorMove()
//    {
//        //// Gerakan murni dari animasi
//        //transform.position += animator.deltaPosition;

//        //// Sinkronkan ke NavMesh
//        //agent.nextPosition = transform.position;

//        transform.position = agent.nextPosition;
//    }
//}

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class CharacterPathfind : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent agent;
    public Animator animator;

    Vector3 targetPosition;
    bool hasTarget = false;

    void Start()
    {
        agent.updatePosition = false;
        agent.updateRotation = false;
    }

    void Update()
    {
        HandleInput();
        HandleStopping();
        UpdateAnimation();
        RotateCharacter();
    }

    void HandleInput()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);

                targetPosition = hit.point;
                hasTarget = true;
            }
        }
    }

    void UpdateAnimation()
    {
        if (!agent.hasPath)
        {
            animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
            return;
        }

        float distance = agent.remainingDistance;

        if (distance <= agent.stoppingDistance)
        {
            animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
            return;
        }

        Vector3 direction = agent.desiredVelocity;
        float speed = Mathf.Clamp01(direction.magnitude);

        animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
    }

    void RotateCharacter()
    {
        if (!hasTarget) return;

        Vector3 direction = agent.desiredVelocity;

        if (direction.magnitude > 0.1f)
        {
            Quaternion rot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 10);
        }
    }

    void OnAnimatorMove()
    {
        if (!hasTarget) return;

        transform.position += animator.deltaPosition;
        agent.nextPosition = transform.position;
    }

    void HandleStopping()
    {
        if (!hasTarget) return;

        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance < 0.3f) // bisa kamu adjust (0.15 - 0.3)
        {
            hasTarget = false;
            agent.ResetPath();

            animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
        }
    }
}
