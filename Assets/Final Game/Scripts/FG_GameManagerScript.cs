using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FG_GameManagerScript : MonoBehaviour
{
    public int health = 20;
    public int money = 120;
    public int maxWave = 7;
    public int currentWave = 0;
    public float timeBetweenWave = 11f;
    public float remainingTime;

    public List<GameObject> enemiesPrefab;

    public LinkedList<GameObject> enemies = new();

    public FG_GameUiScript gameUI;

    private bool startNewWave = false;
    private bool waitForNewWave = true;

    // Start is called before the first frame update
    void Start()
    {
        remainingTime = timeBetweenWave;
        gameUI = GameObject.FindGameObjectWithTag("GameUI").GetComponent<FG_GameUiScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemies.Count == 0)
        {
            if (startNewWave)
            {
                currentWave++;

                startNewWave = false;
                waitForNewWave = true;
                remainingTime = timeBetweenWave;
                gameUI.UpdateWaveText(currentWave.ToString(), maxWave.ToString());
                StopCoroutine("StartNewWaveCountDown");
                ManageWave();
            } else if (waitForNewWave)
            {
                StartCoroutine("StartNewWaveCountDown");
                remainingTime -= Time.deltaTime;
                gameUI.WaveCountDown(Mathf.FloorToInt(remainingTime % 60));
            }
        }
    }

    public void RemoveHealth(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            gameUI.DisplayLoseScreen();
        }
        gameUI.UpdateHealthText(health.ToString());
    }

    public void AddMoney(int moneyToAdd)
    {
        money += moneyToAdd;
        gameUI.UpdateMoneyText(money.ToString());
    }

    public bool RemoveMoney(int moneyToRemove)
    {
        if (money - moneyToRemove >= 0)
        {
            money -= moneyToRemove;
            gameUI.UpdateMoneyText(money.ToString());
            return true;
        }
        return false;
    }

    private void ManageWave()
    {
        switch (currentWave)
        {
            case 1:
                StartCoroutine(RandomSpawnGobelin(10));
                break;
            case 2:
                StartCoroutine(RandomSpawnHornet(10));
                break;
            case 3:
                StartCoroutine(RandomSpawnWolf(10));
                break;
            case 4:
                StartCoroutine(RandomSpawnGobelin(10));
                StartCoroutine(RandomSpawnHornet(7));
                StartCoroutine(RandomSpawnWolf(5));
                break;
            case 5:
                StartCoroutine(RandomSpawnGobelin(7));
                StartCoroutine(RandomSpawnHornet(5));
                StartCoroutine(RandomSpawnWolf(3));
                enemies.AddLast(Instantiate(enemiesPrefab[3], GameObject.FindGameObjectWithTag("SpawnPointB").transform.position, GameObject.FindGameObjectWithTag("SpawnPointB").transform.rotation));
                enemies.Last().GetComponent<FG_EnemyPath>().speed = Random.Range(0.8f, 1.4f);
                break;
            case 6:
                StartCoroutine(RandomSpawnGobelin(10));
                StartCoroutine(RandomSpawnHornet(7));
                StartCoroutine(RandomSpawnWolf(5));
                enemies.AddLast(Instantiate(enemiesPrefab[3], GameObject.FindGameObjectWithTag("SpawnPointB").transform.position, GameObject.FindGameObjectWithTag("SpawnPointB").transform.rotation));
                enemies.Last().GetComponent<FG_EnemyPath>().speed = Random.Range(0.8f, 1.4f);
                enemies.AddLast(Instantiate(enemiesPrefab[4], GameObject.FindGameObjectWithTag("SpawnPointA").transform.position, GameObject.FindGameObjectWithTag("SpawnPointA").transform.rotation));
                enemies.Last().GetComponent<FG_EnemyPath>().speed = Random.Range(1.1f, 1.7f);
                break;
            case 7:
                StartCoroutine(RandomSpawnGobelin(15));
                StartCoroutine(RandomSpawnHornet(12));
                StartCoroutine(RandomSpawnWolf(10));

                enemies.AddLast(Instantiate(enemiesPrefab[3], GameObject.FindGameObjectWithTag("SpawnPointB").transform.position, GameObject.FindGameObjectWithTag("SpawnPointB").transform.rotation));
                enemies.Last().GetComponent<FG_EnemyPath>().speed = Random.Range(0.8f, 1.4f);

                enemies.AddLast(Instantiate(enemiesPrefab[4], GameObject.FindGameObjectWithTag("SpawnPointA").transform.position, GameObject.FindGameObjectWithTag("SpawnPointA").transform.rotation));
                enemies.Last().GetComponent<FG_EnemyPath>().speed = Random.Range(1.1f, 1.7f);
                break;
        }
    }

    IEnumerator StartNewWaveCountDown()
    {
        if (currentWave + 1 > maxWave) // If it next wave is over the max wave (previous wave was the last)
        {
            gameUI.DisplayWinScreen();
            yield return null;
        }
        yield return new WaitForSeconds(timeBetweenWave);
        startNewWave = true;
        waitForNewWave = false;
    }

    IEnumerator RandomSpawnGobelin(int numberOfSpawn)
    {
        for (int i = 0; i < numberOfSpawn; i++)
        {
            enemies.AddLast(Instantiate(enemiesPrefab[0], GameObject.FindGameObjectWithTag("SpawnPointB").transform.position, GameObject.FindGameObjectWithTag("SpawnPointB").transform.rotation));
            enemies.Last().GetComponent<FG_EnemyPath>().speed = Random.Range(0.7f, 1.3f);
            yield return new WaitForSeconds(Random.Range(0.5f, 1f));
        }
    }

    IEnumerator RandomSpawnHornet(int numberOfSpawn)
    {
        for (int i = 0; i < numberOfSpawn; i++)
        {
            enemies.AddLast(Instantiate(enemiesPrefab[1], GameObject.FindGameObjectWithTag("SpawnPointA").transform.position, GameObject.FindGameObjectWithTag("SpawnPointA").transform.rotation));
            enemies.Last().GetComponent<FG_EnemyPath>().speed = Random.Range(1.3f, 1.9f);
            yield return new WaitForSeconds(Random.Range(0.5f, 1f));
        }
    }

    IEnumerator RandomSpawnWolf(int numberOfSpawn)
    {
        for (int i = 0; i < numberOfSpawn; i++)
        {
            enemies.AddLast(Instantiate(enemiesPrefab[2], GameObject.FindGameObjectWithTag("SpawnPointB").transform.position, GameObject.FindGameObjectWithTag("SpawnPointB").transform.rotation));
            enemies.Last().GetComponent<FG_EnemyPath>().speed = Random.Range(1f, 1.6f);
            yield return new WaitForSeconds(Random.Range(0.5f, 1f));
        }
    }
}
