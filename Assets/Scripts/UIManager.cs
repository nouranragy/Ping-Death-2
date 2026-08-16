using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    //public TextMeshProUGUI livesText;
    // public TextMeshProUGUI gameOverText;
    [SerializeField] private GameObject[] heartImages;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI chainCounterText;



    //public Button pauseButton;
    //public Button restartButton;
    [SerializeField] private Button nextLevelButton;

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
        ChainManager.OnChainUpdated += UpdateChainUI;
    }

    private void OnDisable()
    {
        GameManager.OnLivesChanged -= UpdateLivesUI;
        GameManager.OnTimerUpdated -= UpdateTimerUI;
        GameManager.OnPauseStateChanged -= ShowPausePanel;
        GameManager.OnGameOver -= ShowGameOverPanel;
        GameManager.OnGameWin -= ShowWinPanel;
        ChainManager.OnChainUpdated -= UpdateChainUI;
    }

    private void UpdateLivesUI(int newLives)
    {
        //livesText.text = "Lives: " + newLives;
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < newLives)
            {
                heartImages[i].SetActive(true);
            }
            else
            {
                heartImages[i].SetActive(false);
            }
        }
    }

    private void UpdateTimerUI(float newTime)
    {
        timerText.text = Mathf.Ceil(newTime).ToString();
    }

    private void UpdateChainUI(int connected, int total)
    {
        if (chainCounterText != null)
        {
            chainCounterText.text = "CHAIN "+connected;
        }
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
        Time.timeScale = 0f;

        WinPanel.SetActive(true);
        ShowNextLevelButton();
    }

    private void ShowNextLevelButton()
    {
        int nextSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1;
        bool hasNextLevel = nextSceneIndex < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        nextLevelButton.gameObject.SetActive(hasNextLevel);
    }

    public void OnNextLevelButtonClicked()
    {
        if (LevelManager.Instance != null)
            LevelManager.Instance.LoadNextLevel();
    }

    public void OnRestartButtonClicked()
    {
        if (LevelManager.Instance != null)
            LevelManager.Instance.RestartCurrentLevel();
    }

    public void OnMainMenuButtonClicked()
    {
        if (LevelManager.Instance != null)
            LevelManager.Instance.LoadMainMenu();
    }

    public void OnResumeButtonClicked()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.TogglePause();
    }



    


}