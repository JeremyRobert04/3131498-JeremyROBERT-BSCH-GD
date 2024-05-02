using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FG_EnemyDeathEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void KillEnemy()
    {
        GetComponentInParent<FG_EnemyPath>().DeadEnemy();
    }
}
