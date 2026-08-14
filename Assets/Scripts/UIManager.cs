using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI timerText;
    // public TextMeshProUGUI gameOverText;


    //public Button pauseButton;
    //public Button restartButton;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject WinPanel;


    private void OnEnable()
    {
        GameManager.OnLivesChanged += UpdateLivesUI;
        GameManager.OnTimerUpdated += UpdateTimerUI;
        GameManager.OnPauseStateChanged += ShowPausePanel;
        GameManager.OnGameOver += ShowGameOverPanel;
        GameManager.OnGameWin += ShowWinPanel;
    }

    private void OnDisable()
    {
        GameManager.OnLivesChanged -= UpdateLivesUI;
        GameManager.OnTimerUpdated -= UpdateTimerUI;
        GameManager.OnPauseStateChanged -= ShowPausePanel;
        GameManager.OnGameOver -= ShowGameOverPanel;
        GameManager.OnGameWin -= ShowWinPanel;
    }

    private void UpdateLivesUI(int newLives)
    {
        livesText.text = "Lives: " + newLives;
    }

    private void UpdateTimerUI(float newTime)
    {
        timerText.text = "Time: " + Mathf.Ceil(newTime).ToString();
    }

    private void ShowPausePanel(bool isPaused)
    { 
        pausePanel.SetActive(isPaused);
    }

    private void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);

    }

    public void ShowWinPanel()
    {       
        WinPanel.SetActive(true);
        Time.timeScale = 0f;    
    }

    public void OnResumeButtonClicked()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.TogglePause();
    }



}