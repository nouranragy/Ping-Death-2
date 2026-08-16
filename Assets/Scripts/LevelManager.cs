using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

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

    
    public void RestartCurrentLevel()
    {
        Time.timeScale = 1f;

        if (!GameManager.Instance.isGameActive)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
    }

    
    public void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        LoadLevel(nextSceneIndex);

    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadLevel(int levelIndex)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelIndex);
    }
}