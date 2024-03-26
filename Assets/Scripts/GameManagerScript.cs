using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{

    public int health;
    public float score;
    public float invulnerableCooldown;
    public bool invulnerable = false;
    public Transform spawnPoint;
    public CharacterControllerScript playerScript;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterControllerScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddScore(float scoreToAdd)
    {
        score += scoreToAdd;
    }

    public void TakeDamage(int healthToRemove)
    {
        if (!invulnerable && health > 0)
        {
            health -= healthToRemove;
            playerScript.anim.SetTrigger("isTakingDamage");
            if (health <= 0)
            {
                Destroy(GameObject.FindWithTag("Player"));
            }
            StartCoroutine(InvulnerabilityCooldown());
        }
    }

    IEnumerator InvulnerabilityCooldown()
    {
        invulnerable = true;
        yield return new WaitForSeconds(invulnerableCooldown);
        invulnerable = false;
    }
}
