using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips - SFX")]
    public AudioClip gameOverSound;
    public AudioClip winSound;
    public AudioClip buttonClickSound;

    [Header("Audio Clips - Music")]
    public AudioClip backgroundMusic;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mainMixer;

    [SerializeField] private float maxMusicVolume = 0.15f; 
    [SerializeField] private float maxSFXVolume = 0.25f;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateVolumeSettings();
        PlayMusic(backgroundMusic);
    }

    private void OnEnable()
    {
        
        GameManager.OnGameWin += PlayWinSound;
        GameManager.OnGameOver += PlayGameOverSound;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        GameManager.OnGameWin -= PlayWinSound;
        GameManager.OnGameOver -= PlayGameOverSound;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sfxSource.Stop(); 
 
        if (backgroundMusic != null)
        {
            PlayMusic(backgroundMusic);
        }
    }
    public void PlaySFX(AudioClip clip)
    {
        
        sfxSource.PlayOneShot(clip);
        
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
        
    }

    private void PlayWinSound()
    {
        musicSource.Stop(); 
        sfxSource.PlayOneShot(winSound);
    }

    private void PlayGameOverSound(string gameOverReason)
    {
        musicSource.Stop();
        sfxSource.PlayOneShot(gameOverSound);
    }

    public void PlayButtonClick()
    {
        PlaySFX(buttonClickSound);
    }
    public void SetMusicVolume(float value)
    {
        float cappedValue = Mathf.Clamp01(value) * maxMusicVolume;
        musicSource.volume = cappedValue;
        
        PlayerPrefs.SetFloat("MusicVolume", value);
    }
    public void SetSFXVolume(float value)
    {
        float cappedValue = Mathf.Clamp01(value) * maxSFXVolume;
        sfxSource.volume = cappedValue;
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    private void UpdateVolumeSettings()
    {
        
        float savedMusicVol = PlayerPrefs.GetFloat("MusicVolume",1f);
        float savedSFXVol = PlayerPrefs.GetFloat("SFXVolume",1f);


        SetMusicVolume(savedMusicVol);
        SetSFXVolume(savedSFXVol);
    }
}