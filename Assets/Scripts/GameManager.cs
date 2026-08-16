
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }


    private int currentLives;
    [SerializeField] public int maxLives = 3;
    [SerializeField] public float levelTimer = 60f;
    

     public bool isGameActive = false;
     public bool isPaused=false;
    
    public static event Action<int> OnLivesChanged;
    public static event Action<float> OnTimerUpdated;
    public static event Action<string> OnGameOver;
    public static event Action OnGameWin;
    public static event Action<bool> OnPauseStateChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    
    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (!isGameActive) return;

        levelTimer -= Time.deltaTime;
        OnTimerUpdated?.Invoke(levelTimer);

        if (levelTimer <= 0)
        {
            GameOver("Out Of Time!");
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        currentLives = maxLives;
        isGameActive = true;
        OnLivesChanged?.Invoke(currentLives);
        OnTimerUpdated?.Invoke(levelTimer);
    }

    public void LoseLife()
    {
        

        currentLives--;
        OnLivesChanged?.Invoke(currentLives);

        if (currentLives <= 0)
        {
            GameOver("Out Of Lives!");
        }
        
    }

    public void LevelWin()
    {
        Debug.Log("Level Completed! All Random Nodes Connected!");

        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("UnlockedLevel",currentSceneIndex );
        
        isGameActive = false;
        OnGameWin?.Invoke();

    }

    private void GameOver(string gameOverReason )
    {
        if (!isGameActive) return;
        isGameActive = false;
        OnGameOver?.Invoke(gameOverReason);
    }
    
    
    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f; 
        OnPauseStateChanged?.Invoke(isPaused);
    }

}
