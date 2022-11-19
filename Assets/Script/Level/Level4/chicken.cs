using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class chicken : MonoBehaviour
{
    public bool isCatched = false;

    private NavMeshAgent navMeshAgent;
    public LayerMask whatIsGround;
    public Transform target;
    public float nearTarget;
    private bool isNearTarget;
    public LayerMask targetLayer;

    private Animator animator;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        target = GameObject.Find("Target").transform;
    }

    private void Update()
    {
        isNearTarget = Physics.CheckSphere(transform.position, nearTarget, targetLayer);
        if (isCatched)
        {
            if (!isNearTarget)
            {
                ChaseTarget();
            }
            else
            {
                animator.SetBool("Running", false);
                navMeshAgent.speed = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().CompareTag("Player") && Input.GetButtonDown("Skill"))
        {
            Debug.Log("player");
        }
    }

    public void CatchChicken()
    {
        if (!isCatched)
        {
            Debug.Log("Catch");
            isCatched = true;
            animator.SetTrigger("Jump");
        }
    }

    private void ChaseTarget()
    {
        navMeshAgent.speed = 2;
        navMeshAgent.destination = target.transform.position;
        animator.SetBool("Running", true);
    }
}
