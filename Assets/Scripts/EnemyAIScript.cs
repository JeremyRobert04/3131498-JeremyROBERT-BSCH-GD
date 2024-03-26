using System;
using System.Collections;
using UnityEngine;

public class EnemyAIScript : MonoBehaviour
{
    public enum State
    {
        Patrol,
        DetectPlayer,
        Chasing,
    }

    public State enemyAIState;
    public int damage;
    public float patrollingSpeed;
    public float chassingSpeed;
    public bool playerInDetectionZone;
    public float aggroTimer;

    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;


    // Start is called before the first frame update
    void Start()
    {
        enemyAIState = State.Patrol;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        currentPoint = pointA.transform;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

        switch (enemyAIState)
        {
            case State.Patrol:
                HandlePatrolling();
                break;
            case State.DetectPlayer:
                break;
            case State.Chasing:
                HandleChassing();
                break;
        }
    }

    private void HandleChassing()
    {
        Transform playerPos = GameObject.FindGameObjectWithTag("Player").transform;

        if (playerPos.position.x < transform.position.x)
        {
            // If sprit is facing right then flip it to face left
            if (transform.localScale.x == -1)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

            // Player is on the left side compared to the enemy
            // Check that the mob does not go beyond its patrol point
            if (transform.position.x - pointA.transform.position.x > 1f)
            {
                rb.velocity = new Vector2(-chassingSpeed, 0);
            }
        }
        else
        {
            // If sprit is facing left then flip to face right
            if (transform.localScale.x == 1)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            // Player is on the right side compared to the enemy
            // Check that the mob does not go beyond its patrol point
            if (pointB.transform.position.x - transform.position.x > 1f)
            {
                rb.velocity = new Vector2(chassingSpeed, 0);
            }
        }
    }

    private void HandlePatrolling()
    {
        if (currentPoint == pointB.transform)
        {
            rb.velocity = new Vector2(patrollingSpeed, 0);
            if (transform.localScale.x == 1)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            rb.velocity = new Vector2(-patrollingSpeed, 0);
            if (transform.localScale.x == -1)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            transform.localScale = new Vector3(1, 1, 1);
            currentPoint = pointA.transform;
        }
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            transform.localScale = new Vector3(-1, 1, 1);;
            currentPoint = pointB.transform;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player")) // If the player enter the detection Zone
        {
            playerInDetectionZone = true;

            if (col.transform.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            } else {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            StopCoroutine(PlayerTriggerAggroZone());
            StartCoroutine(PlayerTriggerAggroZone());
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player")) // If the player exit the detection Zone
        {
            playerInDetectionZone = false;
            if (enemyAIState == State.Chasing)
            {
                StopCoroutine(PlayerTriggerAggroZone());
                StartCoroutine(PlayerTriggerAggroZone());
            }
        }
    }

    IEnumerator PlayerTriggerAggroZone()
    {
        enemyAIState = State.DetectPlayer;
        yield return new WaitForSeconds(aggroTimer);

        if (playerInDetectionZone)
        {
            enemyAIState = State.Chasing;
            anim.SetBool("Chassing", true);
        }
        else
        {
            enemyAIState = State.Patrol;
            anim.SetBool("Chassing", false);
        }
    }
}