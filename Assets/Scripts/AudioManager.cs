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

    private void PlayGameOverSound()
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
   
        mainMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Max(0.0001f, value)) * 20);
    }
    public void SetSFXVolume(float value)
    {
        mainMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Max(0.0001f, value)) * 20);
    }
}