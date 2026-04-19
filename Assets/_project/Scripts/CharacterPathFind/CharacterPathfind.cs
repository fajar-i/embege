using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class CharacterPathfind : MonoBehaviour
{
    [Header("References")]
    public Camera cam;
    public NavMeshAgent agent;
    public Animator animator;

    [Header("Settings")]
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float stopDistanceThreshold = 0.3f;

    private Vector3 targetPosition;
    private bool hasTarget = false;

    void Start()
    {
        // Mematikan kontrol otomatis agar bisa dikontrol lewat OnAnimatorMove (Root Motion)
        agent.updatePosition = false;
        agent.updateRotation = false;
    }

    void Update()
    {
        HandleInput();
        UpdateAnimationAndStatus();
        RotateCharacter();
    }

    void HandleInput()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out var hit))
            {
                agent.SetDestination(hit.point);
                targetPosition = hit.point;
                hasTarget = true;
            }
        }
    }

    void UpdateAnimationAndStatus()
    {
        if (!hasTarget) return;

        // Cek apakah sudah sampai tujuan secara manual (karena updatePosition = false)
        float distance = Vector3.Distance(transform.position, targetPosition);
        
        if (distance < stopDistanceThreshold)
        {
            StopCharacter();
        }
        else
        {
            // Mengatur parameter Speed berdasarkan kecepatan agent yang diinginkan
            float speed = Mathf.Clamp01(agent.desiredVelocity.magnitude);
            animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
        }
    }

    void RotateCharacter()
    {
        if (!hasTarget || agent.desiredVelocity.sqrMagnitude < 0.1f) return;

        Quaternion lookRotation = Quaternion.LookRotation(agent.desiredVelocity);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    void OnAnimatorMove()
    {
        // Menyinkronkan posisi transform dengan delta animasi
        if (hasTarget)
        {
            transform.position += animator.deltaPosition;
            agent.nextPosition = transform.position; // Penting: agar NavMesh Agent tetap sinkron dengan model
        }
    }

    private void StopCharacter()
    {
        hasTarget = false;
        agent.ResetPath();
        animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }
}