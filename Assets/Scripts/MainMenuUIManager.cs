using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
   
    [SerializeField] private GameObject levelSelectPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject mainMenuPanel;

    [SerializeField] private Button level2Button;
    [SerializeField] private Button level3Button;

    private void Start()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

         level2Button.interactable = (unlockedLevel >= 2);
         level3Button.interactable = (unlockedLevel >= 3);
    }

    public void OnPlayButtonClicked()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true); 

    }

    public void OnSettingsButtonClicked()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true); 
    }

    public void OnQuitButtonClicked()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif 
    }

    
    public void OnCloseLevelSelect()
    {
        levelSelectPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void OnCloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    public void SelectLevel(int levelIndex)
    {
        LevelManager.Instance.LoadLevel(levelIndex);

    }


    //settingsPanel

    public void EditSounVolume(float volume)
    {
        AudioListener.volume = volume;
    }


}
