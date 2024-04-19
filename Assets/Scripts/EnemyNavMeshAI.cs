using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMeshAI : MonoBehaviour
{
    public Transform[] patrolPoint;
    public Animator animator;
    public UnityEngine.AI.NavMeshAgent agent;
    public float currentVelocity;
    public Transform player;
    public bool aggro;
    private bool destinationReach;
    public float destinationThreshold;
    public float aggroTimer;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        aggro = false;
        destinationReach = true;
    }

    // Update is called once per frame
    void Update()
    {
        currentVelocity = agent.velocity.magnitude;
        animator.SetFloat("velocity", currentVelocity);

        if (aggro)
        {
            agent.destination = player.position;
        } else if (!aggro && destinationReach) {
            destinationReach = false;
            agent.destination = patrolPoint[Random.Range(0, patrolPoint.Length)].position;
        }

        if (Vector3.Distance(transform.position, agent.destination) < destinationThreshold)
        {
            destinationReach = true;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            aggro = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            agent.destination = player.position; //this is the last seen player position
            StopCoroutine("AggroTimer");
            StartCoroutine("AggroTimer");
        }
    }

    IEnumerator AggroTimer()
    {
        yield return new WaitForSeconds(aggroTimer);
        aggro = false;
        destinationReach = true;
    }
}
