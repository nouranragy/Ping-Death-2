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
    [SerializeField] private float maxDashDuration = 0.2f;

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

    }
    
    
    public void OnDash(InputValue value)
    {
        if (value.isPressed && canDash && !isDashing)
        {
            if (!GameManager.Instance.isGameActive) { return; }
            // Stop any leftover coroutine before starting a new one
            if (dashCoroutine != null)
            {
                StopCoroutine(dashCoroutine);
            }

            AudioManager.Instance.PlaySFX(dashSound);

            // Store coroutine reference to manage execution
         dashCoroutine =   StartCoroutine(DashToExactMousePointer());
        }
    }
    private IEnumerator DashToExactMousePointer()
    {
        canDash = false;
        isDashing = true;

        OnPlayerDashed?.Invoke();

        if (anim != null & GameManager.Instance.isGameActive)    anim.SetBool("isDashing", true);

          rb.linearVelocity = Vector2.zero;
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(mouseScreenPos);
        Vector2 targetPosition = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
        
        Vector2 dashDirection = (targetPosition - (Vector2)transform.position).normalized;

        if (anim != null & GameManager.Instance.isGameActive)
        {
            anim.SetFloat("DashX", dashDirection.x);
            anim.SetFloat("DashY", dashDirection.y);
            anim.SetTrigger("Dash"); 
        }

        //float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        float elapsedTime = 0f;

        while (Vector2.Distance(rb.position, targetPosition) > 0.1f && elapsedTime < maxDashDuration)
        {
            Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, dashSpeed * Time.deltaTime);
            rb.MovePosition(newPosition);

            elapsedTime += Time.deltaTime;
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing)
        {
            if (dashCoroutine != null)
            {
                StopCoroutine(dashCoroutine);
            }
            StopDashPhysics();
            StartCoroutine(ResetDashCooldown());
        }
    }
    private IEnumerator ResetDashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    

}


