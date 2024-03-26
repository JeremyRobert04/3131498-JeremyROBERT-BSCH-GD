using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageCollision : MonoBehaviour
{
    EnemyAIScript script;

    // Start is called before the first frame update
    void Start()
    {
        script = GetComponentInParent<EnemyAIScript>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>().TakeDamage(script.damage);
        }
    }
}
