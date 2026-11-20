using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine.UnityConsent;

public class SampleGameManager : MonoBehaviour
{
    public static SampleGameManager Instance;
    public TMP_Text scoreText;
    public TMP_Text timeText;

    public int currentScore=0;
    public CoinSampleGame coin;
    public Transform player;
    public GameObject boomEffect;
    public GameObject thorns;
    public FinishSampleGame finish;
    public TimeCounterSampleGame timeCounter;

    public GameObject gameoverPanel;
    public GameObject youwinPanel;
    public int level = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void GameOver()
    {
        currentScore = 0;
        gameoverPanel.gameObject.SetActive(true);
        boomEffect.transform.position = player.transform.position;
        boomEffect.SetActive(true);
        player.gameObject.SetActive(false);
        timeCounter.isGameOver = true;

        if (UnityServices.State != ServicesInitializationState.Initialized || 
            EndUserConsent.GetConsentState().AnalyticsIntent != ConsentStatus.Granted)
        {
            return;
        }

        CustomEvent levelDiedEvent = new CustomEvent("LevelDied")
        {
            {"userLevel", level },
            {"posX", transform.position.x },
            {"posY", transform.position.y },
            {"coinCount", currentScore }
        };
        
        AnalyticsService.Instance.RecordEvent(levelDiedEvent);
        AnalyticsService.Instance.Flush();
    }
    public void YouWin()
    {
        youwinPanel.gameObject.SetActive(true);
        player.gameObject.SetActive(false);
        thorns.gameObject.SetActive(false);
        coin.gameObject.SetActive(false);
        timeCounter.isGameOver = true;
        
        if (UnityServices.State != ServicesInitializationState.Initialized ||
            EndUserConsent.GetConsentState().AnalyticsIntent != ConsentStatus.Granted)
        {
            return;
        }

        CustomEvent levelWinEvent = new CustomEvent("LevelWin")
        {
            {"userLevel",level },
            {"timeCount",timeCounter.GetCurrentTime },
            {"coinCount",currentScore }
        };
        AnalyticsService.Instance.RecordEvent(levelWinEvent);
        AnalyticsService.Instance.Flush();
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddScore()
    {
        currentScore += 1;
        scoreText.text = currentScore.ToString("0");

        if(currentScore %3 == 0)
        {
            SpawnFinishLine();
        }
    }
    
    public void SpawnFinishLine()
    {
        finish.RandomSpawn();
    }

    // Start is called before the first frame update
    void Start()
    {
        coin.RandomSpawn();
    }
    
}
