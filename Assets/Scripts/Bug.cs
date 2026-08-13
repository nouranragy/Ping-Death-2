using UnityEngine;

public class Bug : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hitPlayerSound;
    private void Awake()
    {
        if(audioSource == null)
        audioSource = GetComponent<AudioSource>();
    }
    protected void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            OnHitPlayer(other.gameObject);
        }
    }

    private void OnHitPlayer(GameObject player)
    {
        if(audioSource != null && hitPlayerSound != null)
        {
            AudioSource.PlayClipAtPoint(hitPlayerSound, transform.position);
        }
        GameManager.Instance.LoseLife();
        Debug.Log("Player took damage");
    }
}
