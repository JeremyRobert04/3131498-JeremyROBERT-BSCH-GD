using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FG_EnemyPath : MonoBehaviour
{
    public List<Vector3> patrolPoints;
    private int currentPatrolIndex = 0;

    private Animator anim;
    private FG_GameManagerScript gameManager;

    public float speed;
    public int moneyWorth;
    public int hp;
    public int damageDeal;

    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);

        anim = GetComponentInChildren<Animator>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<FG_GameManagerScript>();

        // Get all points with tags CheckPoints and assign them to the list of patrolPoints
        GameObject[] patrolPointObjects = FindObsWithTag("CheckPoint");

        foreach (GameObject patrolPointObject in patrolPointObjects)
        {
            Vector3 newRandomPatrolPoint = patrolPointObject.transform.position + new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.4f, 0.4f), 0);
            patrolPoints.Add(newRandomPatrolPoint);
        }

        patrolPoints.Add(GameObject.FindGameObjectWithTag("Finish").transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (patrolPoints.Count == 0)
        {
            Debug.LogWarning("No patrol points has been defined!");
            return;
        }

        Vector3 direction = (patrolPoints[currentPatrolIndex] - transform.position).normalized;

        if (direction.x > 0.5f)
        {
            if (transform.localScale.x == 1)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            anim.SetBool("Left", true);
            anim.SetBool("Up", false);
            anim.SetBool("Down", false);
        } else if (direction.y > 0.5f)
        {
            anim.SetBool("Left", false);
            anim.SetBool("Up", true);
            anim.SetBool("Down", false);
        } else if (direction.y < -0.5f)
        {
            anim.SetBool("Left", false);
            anim.SetBool("Up", false);
            anim.SetBool("Down", true);
        }

        if (isDead)
        {
            return;
        }

        transform.Translate(speed * Time.deltaTime * direction);

        if (Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex]) < 0.1f)
        {
            currentPatrolIndex += 1;
        }

        // Has reach the end
        if (currentPatrolIndex >= patrolPoints.Count)
        {
            gameManager.RemoveHealth(damageDeal);
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        gameManager.enemies.Remove(gameObject);
    }

    public void TakeDamage(int damageTake)
    {
        hp -= damageTake;
        if (hp <= 0)
        {
            isDead = true;
            gameManager.AddMoney(moneyWorth);
            anim.SetTrigger("Dead");
        }
    }

    public void DeadEnemy()
    {
        Destroy(gameObject);
    }

    /*
    void OnDrawGizmos()
    {
        if (patrolPoints.Count == 0)
        {
            return;
        }

        for (int i = 0; i < patrolPoints.Count - 1; i++)
        {
            Gizmos.DrawWireSphere(patrolPoints[i], 0.5f);
            Gizmos.DrawLine(patrolPoints[i], patrolPoints[i + 1]);
        }

        Gizmos.DrawWireSphere(patrolPoints.Last(), 0.5f);
    }
    */

    GameObject[] FindObsWithTag( string tag )
    {
        GameObject[] foundObs = GameObject.FindGameObjectsWithTag(tag);
        System.Array.Sort( foundObs, CompareObNames );
        return foundObs;
    }

    int CompareObNames( GameObject x, GameObject y )
    {
        return x.name.CompareTo( y.name );
    }
}
