
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
    [SerializeField] private float invincibilityDuration = 1f;


    public bool isGameActive = false;
    public bool isPaused=false;
    private bool isInvincible = false;


    public static event Action<int> OnLivesChanged;
    public static event Action<float> OnTimerUpdated;
    public static event Action  OnGameOver;
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
        DontDestroyOnLoad(gameObject);
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
            GameOver();
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
        if (isInvincible) return;

        currentLives--;
        OnLivesChanged?.Invoke(currentLives);

        if (currentLives <= 0)
        {
            GameOver();
        }
        else
        {
            StartCoroutine(InvincibilityRoutine());
        }
    }

    public void LevelWin()
    {
        Debug.Log("Level Completed! All Random Nodes Connected!");
        isGameActive = false;
        OnGameWin?.Invoke();
    }

    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;

        yield return new WaitForSeconds(invincibilityDuration);

        isInvincible = false;
    }

    private void GameOver()
    {
        isGameActive = false;
        OnGameOver?.Invoke();
    }

    public void RestartCurrentLevel()
    {
        StartGame();
        //Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f; 
        OnPauseStateChanged?.Invoke(isPaused);
    }


}
