using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FG_ArcherTowerScript : MonoBehaviour
{
    public int damage;
    public float rateOfFire;
    public int towerLevel = 0;
    public int[] towerUpgradeCost;

    private bool isMaxed = false;

    public LinkedList<GameObject> enemies = new();
    public GameObject projectile;

    private Animator anim;

    private bool isShooting = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemies.Count == 0 && isShooting)
        {
            StopCoroutine("ShotArrow");
            isShooting = false;
        }
    }

    public bool IsMaxed()
    {
        return isMaxed;
    }

    public int GetUpgradeCost()
    {
        return towerUpgradeCost[towerLevel];
    }

    public void UpgradeTower()
    {
        anim.SetTrigger("Upgrade");

        towerLevel++;
        if (towerLevel > towerUpgradeCost.Length)
        {
            isMaxed = true;
        }

        switch (towerLevel)
        {
            case 1:
                damage += 1;
                break;
            case 2:
                rateOfFire -= 0.25f;
                break;
            case 3:
                damage += 1;
                break;
            case 4:
                rateOfFire -= 0.25f;
                break;
            case 5:
                damage += 2;
                break;
            case 6:
                rateOfFire -= 0.5f;
                break;
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            enemies.AddLast(col.gameObject);
            if (!isShooting)
            {
                isShooting = true;
                StartCoroutine("ShotArrow");
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            enemies.Remove(col.gameObject);
        }
    }

    IEnumerator ShotArrow()
    {
        while (true)
        {
            if (enemies.Count > 0)
            {
                GameObject arrow = Instantiate(projectile, transform.position, transform.rotation);
                arrow.GetComponent<FG_ProjectileScript>().SetTarget(enemies.First());
                arrow.GetComponent<FG_ProjectileScript>().SetDamage(damage);
            }
            yield return new WaitForSeconds(rateOfFire);
        }
    }
}
