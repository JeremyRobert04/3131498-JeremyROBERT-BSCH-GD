using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FG_ProjectileScript : MonoBehaviour
{
    private GameObject target;
    private int projectilDamage;
    public float speed = 6f;

    private FG_ArrowRotateScript spriteRotation;

    private bool isTargetInstantiate = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRotation = GetComponentInChildren<FG_ArrowRotateScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTargetInstantiate && target == null)
        {
            Destroy(gameObject);
        }

        Vector3 direction = target.transform.position - transform.position;
        direction.Normalize();

        spriteRotation.RotateSprite(target.transform.position);

        transform.Translate(speed * Time.deltaTime * direction);
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
        isTargetInstantiate = true;
    }

    public void SetDamage(int damage)
    {
        projectilDamage = damage;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy") && col.gameObject == target)
        {
            Destroy(gameObject);
            target.GetComponent<FG_EnemyPath>().TakeDamage(projectilDamage);
        }
    }
}