using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FG_GameUiScript : MonoBehaviour
{
    public TMP_Text healthText;
    public TMP_Text moneyText;
    public TMP_Text waveText;
    public TMP_Text upgradeCost;

    public FG_GameManagerScript gameManager;
    public List<Image> pauseResumeImages;

    public List<GameObject> menusCanvas;

    private bool isPaused = false;

    private bool isCreateTowerChoiceOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<FG_GameManagerScript>();

        menusCanvas[0].SetActive(true);
        menusCanvas[1].SetActive(false);
        menusCanvas[2].SetActive(false);
        menusCanvas[3].SetActive(false);
        menusCanvas[4].SetActive(false);

        UpdateHealthText(gameManager.health.ToString());
        UpdateMoneyText(gameManager.money.ToString());
        UpdateWaveText(gameManager.currentWave.ToString(), gameManager.maxWave.ToString());
        pauseResumeImages[1].gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DisplayLoseScreen()
    {
        menusCanvas[1].SetActive(false);
        menusCanvas[2].SetActive(true);
        PauseGame();
    }

    public void DisplayWinScreen()
    {
        menusCanvas[1].SetActive(true);
        menusCanvas[2].SetActive(false);
        PauseGame();
    }

    public void DisplayCreateTowerScreen(bool open)
    {
        isCreateTowerChoiceOpen = open;
        if (isCreateTowerChoiceOpen)
        {
            menusCanvas[3].SetActive(true);
            menusCanvas[4].SetActive(false);
        } else {
            menusCanvas[3].SetActive(false);
            menusCanvas[4].SetActive(false);
        }
    }

    public void DisplayUpgradeTowerScreen(int price)
    {
        upgradeCost.text = price.ToString();

        menusCanvas[3].SetActive(false);
        menusCanvas[4].SetActive(true);
    }

    public void UpdateHealthText(string healthValue)
    {
        healthText.text = healthValue;
    }

    public void UpdateMoneyText(string moneyValue)
    {
        moneyText.text = moneyValue;
    }

    public void UpdateWaveText(string waveValue, string maxWave)
    {
        waveText.text = "WAVE: " + waveValue + "/" + maxWave;
    }

    public void WaveCountDown(int secondRemaining)
    {
        waveText.text = "WAVE IN: " + secondRemaining + "s";
    }

    public void PauseResumeButton()
    {
        isPaused = !isPaused;

        pauseResumeImages[0].gameObject.SetActive(!isPaused);
        pauseResumeImages[1].gameObject.SetActive(isPaused);
        if (isPaused)
        {
            Time.timeScale = 0f;
        } else {
            Time.timeScale = 1f;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Final Project");
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
}
