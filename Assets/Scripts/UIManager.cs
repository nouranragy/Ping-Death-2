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

    public GameObject gameOverPanel;
    public GameObject pausePanel;


    private void OnEnable()
    {
        GameManager.OnLivesChanged += UpdateLivesUI;
        GameManager.OnTimerUpdated += UpdateTimerUI;
        GameManager.OnPauseStateChanged += ShowPausePanel;
        GameManager.OnGameOver += ShowGameOverPanel;
    }

    private void OnDisable()
    {
        GameManager.OnLivesChanged -= UpdateLivesUI;
        GameManager.OnTimerUpdated -= UpdateTimerUI;
        GameManager.OnPauseStateChanged -= ShowPausePanel;
        GameManager.OnGameOver -= ShowGameOverPanel;
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


}