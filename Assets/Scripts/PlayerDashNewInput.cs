using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
public class PlayerDashNewInput : MonoBehaviour
{
    private Rigidbody2D rb;
    private Camera mainCam;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashCooldown = 1f;

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
            // Store coroutine reference to manage execution
         dashCoroutine =   StartCoroutine(DashToExactMousePointer());
        }
    }
    private IEnumerator DashToExactMousePointer()
    {
        canDash = false;
        isDashing = true;

          rb.linearVelocity = Vector2.zero;
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(mouseScreenPos);
        Vector2 targetPosition = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
        

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

        
      
       
        // yield return new WaitForSeconds(dashCooldown);
        
    }


