using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System;
public class PlayerDashNewInput : MonoBehaviour
{
    public static event Action OnPlayerDashed;

    private Animator anim;
    private Rigidbody2D rb;
    private Camera mainCam;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashCooldown = 0f;

    [Header("Audio Settings")] 
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip dashSound;

    private bool canDash = true;
    private bool isDashing;
    
    private Coroutine dashCoroutine; // track active dash coroutine to allow manual cancellation
    private float originalGravity; // Store default gravity scale to restore after dashing
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
        // Cache initial gravity scale setting
        originalGravity = rb.gravityScale;
        anim = GetComponent<Animator>();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }
    
    
    public void OnDash(InputValue value)
    {
        if (value.isPressed && canDash && !isDashing)
        {
            // Stop any leftover coroutine before starting a new one
            if (dashCoroutine != null)
            {
                StopCoroutine(dashCoroutine);
            }

            PlayDashSound();

            // Store coroutine reference to manage execution
         dashCoroutine =   StartCoroutine(DashToExactMousePointer());
        }
    }
    private IEnumerator DashToExactMousePointer()
    {
        canDash = false;
        isDashing = true;

        OnPlayerDashed?.Invoke();

        if (anim != null) anim.SetBool("isDashing", true);

          rb.linearVelocity = Vector2.zero;
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(mouseScreenPos);
        Vector2 targetPosition = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
        
        Vector2 dashDirection = (targetPosition - (Vector2)transform.position).normalized;

        if (anim != null)
        {
            anim.SetFloat("DashX", dashDirection.x);
            anim.SetFloat("DashY", dashDirection.y);
            anim.SetTrigger("Dash"); 
        }

        //float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        while (Vector2.Distance(rb.position, targetPosition) > 0.05f)
        {
            Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, dashSpeed * Time.deltaTime);
            rb.MovePosition(newPosition);
            yield return null;
        }


        // Snap precisely to target position to prevent minor distance jitter
        rb.position = targetPosition;
        // Reset dash physics state
        StopDashPhysics();

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    // Helper method to reset linear velocity and restore original gravity
    private void StopDashPhysics()
    {
        rb.gravityScale = originalGravity;
        rb.linearVelocity = Vector2.zero;
        isDashing = false;
        if (anim != null) anim.SetBool("isDashing", false);
    }

    public void CancelDash()
    {
        // Stop active dash coroutine if running
        if (dashCoroutine != null)
        {
            StopCoroutine(dashCoroutine);
            dashCoroutine = null;
        }
        rb.gravityScale = originalGravity;
         isDashing = false;
         canDash = true;

    }

        private void PlayDashSound()
    {
        if (audioSource != null && dashSound != null)
        {
            audioSource.PlayOneShot(dashSound);
        }
    }
      
       
        // yield return new WaitForSeconds(dashCooldown);
        
    }


