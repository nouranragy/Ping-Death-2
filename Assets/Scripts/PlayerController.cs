using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private float invincibilityDuration = 1f;
    private float flashTime = 0.1f;
   
    private bool isInvincible = false;

    [SerializeField] private AudioClip hitSound;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TriggerFlash()
    {
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        float timer = 0f;
        while (timer < invincibilityDuration)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.2f);
            yield return new WaitForSeconds(flashTime);

            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(flashTime);

            timer += flashTime * 2;
        }
        spriteRenderer.color = Color.white;
        isInvincible = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bug")& !isInvincible)
        {
            OnHit(gameObject);
            StartCoroutine(InvincibilityRoutine());
            TriggerFlash();
            AudioManager.Instance.PlaySFX(hitSound);

        }
    }

    private void OnHit(GameObject player)
    {
        GameManager.Instance.LoseLife();
        Debug.Log("Player took damage");
    }
    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;

        yield return new WaitForSeconds(invincibilityDuration);

        isInvincible = false;
    }
}