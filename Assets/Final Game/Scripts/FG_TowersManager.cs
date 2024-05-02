using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class TowerInfo
{
    public List<Vector3Int> towerPositions;
    public GameObject towerObject;
    public bool isBuilt;

    private int upgradeCost;

    public TowerInfo(
        List<Vector3Int> positions,
        bool isBuilt = false
    )
    {
        towerPositions = positions;
        this.isBuilt = isBuilt;
    }

    public void SetUpgradeCost(int upgradeCost)
    {
        this.upgradeCost = upgradeCost;
    }

    public int GetUpgradeCost()
    {
        return upgradeCost;
    }

    public bool HasCoordinate(Vector3Int coordinate)
    {
        foreach (Vector3Int towerPosition in towerPositions)
        {
            if (coordinate == towerPosition)
            {
                return true;
            }
        }
        return false;
    }
}

public class FG_TowersManager : MonoBehaviour
{
    public Tilemap towersTilemap;
    public List<GameObject> towersPrefabs;

    private List<TowerInfo> towers = new();

    TowerInfo towerSelected = null;
    private FG_GameManagerScript gameManager;

    // Start is called before the first frame update
    void Start()
    {
        towers.Add(new TowerInfo(
            new List<Vector3Int>
            {
                new(-11, 3, 0),
                new(-10, 3, 0),
                new(-11, 2, 0),
                new(-10, 2, 0)
            }
        ));

        towers.Add(new TowerInfo(
            new List<Vector3Int>
            {
                new(-5, -6, 0),
                new(-4, -6, 0),
                new(-5, -7, 0),
                new(-4, -7, 0),
            }
        ));

        towers.Add(new TowerInfo(
            new List<Vector3Int>
            {
                new(-3, 1, 0),
                new(-2, 1, 0),
                new(-3, 0, 0),
                new(-2, 0, 0)
            }
        ));

        towers.Add(new TowerInfo(
            new List<Vector3Int>
            {
                new(3, 4, 0),
                new(4, 4, 0),
                new(3, 3, 0),
                new(4, 3, 0)
            }
        ));

        towers.Add(new TowerInfo(
            new List<Vector3Int>
            {
                new(9, 3, 0),
                new(10, 3, 0),
                new(9, 2, 0),
                new(10, 2, 0)
            }
        ));

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<FG_GameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = towersTilemap.WorldToCell(mousePosition);

            if (towersTilemap.HasTile(cellPosition))
            {
                GetTowerFromCellPosition(cellPosition);
                OpenTowerChoice();
            } else
            {
                GameObject.FindGameObjectWithTag("GameUI").GetComponent<FG_GameUiScript>().DisplayCreateTowerScreen(false);
            }
        }
    }

    private void GetTowerFromCellPosition(Vector3Int cellPosition)
    {
        int i = 0;
        foreach (TowerInfo tower in towers)
        {
            if (tower.HasCoordinate(cellPosition))
            {
                towerSelected = tower;
                break;
            }
            i++;
        }

        if (towerSelected == null)
        {
            Debug.LogError("Error: no tower at coordinate: " + cellPosition + " has been found...");
            return;
        }
    }

    private void OpenTowerChoice()
    {
        if (!towerSelected.isBuilt)
        {
            // Open menu to select a tower
            GameObject.FindGameObjectWithTag("GameUI").GetComponent<FG_GameUiScript>().DisplayCreateTowerScreen(true);
        } else {
            FG_ArcherTowerScript towerScript = towerSelected.towerObject.GetComponent<FG_ArcherTowerScript>();
            if (towerScript.IsMaxed())
            {
                return;
            }
            towerSelected.SetUpgradeCost(towerScript.GetUpgradeCost());
            GameObject.FindGameObjectWithTag("GameUI").GetComponent<FG_GameUiScript>().DisplayUpgradeTowerScreen(towerSelected.GetUpgradeCost());
        }
    }

    public void CreateTower()
    {
        if (!gameManager.RemoveMoney(50))
        {
            return;
        }

        towerSelected.towerObject = Instantiate(
            towersPrefabs[0],
            new Vector3(
                towerSelected.towerPositions[1].x,
                towerSelected.towerPositions[1].y + 1.1f,
                towerSelected.towerPositions[1].z
            ),
            new Quaternion(0, 0, 0, 0)
        );
        towerSelected.isBuilt = true;
        GameObject.FindGameObjectWithTag("GameUI").GetComponent<FG_GameUiScript>().DisplayCreateTowerScreen(false);
    }

    public void UpgradeTower()
    {
        if (!gameManager.RemoveMoney(towerSelected.GetUpgradeCost()))
        {
            return;
        }

        towerSelected.towerObject.GetComponent<FG_ArcherTowerScript>().UpgradeTower();

        GameObject.FindGameObjectWithTag("GameUI").GetComponent<FG_GameUiScript>().DisplayCreateTowerScreen(false);
    }

    public void RemoveEnemy(GameObject enemyToRemove)
    {
        foreach (TowerInfo tower in towers)
        {
            tower.towerObject.GetComponent<FG_ArcherTowerScript>().enemies.Remove(enemyToRemove);
        }
    }
}
