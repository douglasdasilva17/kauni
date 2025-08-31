using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalBrain : MonoBehaviour
{
    public float radius;
    public float timer;
    public LayerMask navMask;

    private Transform target;
    private NavMeshAgent agent;
    private float currentTimer;

    private bool isIdle;
    public float idleTimer;
    private float currentIdleTimer;
    public Animator anim;

    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        currentTimer = timer;
        currentIdleTimer = idleTimer;
    }

    private void Update()
    {
        currentTimer += Time.deltaTime;
        currentIdleTimer += Time.deltaTime;

        if (currentIdleTimer >= idleTimer)
        {
            StartCoroutine(SwitchIdle());
        }

        if (currentTimer >= timer && !isIdle)
        {
            Vector3 newPosition = RandomNavSphere(transform.position, radius, navMask);
            agent.SetDestination(newPosition);
            currentTimer = 0;
        }

        if (isIdle)
        {
            agent.isStopped = true;
            anim.Play("Idle");
        }
        else
        {
            agent.isStopped = false;
            anim.Play("Walk");
        }
    }

    IEnumerator SwitchIdle()
    {
        isIdle = true;
        yield return new WaitForSeconds(3);
        currentIdleTimer = 0;
        isIdle = false;
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layerMask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layerMask);

        return navHit.position;
    }
}
