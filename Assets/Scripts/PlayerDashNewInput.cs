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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
    }
    
    
    public void OnDash(InputValue value)
    {
        if (value.isPressed && canDash && !isDashing)
        {
            StartCoroutine(DashToExactMousePointer());
        }
    }
    private IEnumerator DashToExactMousePointer()
    {
        canDash = false;
        isDashing = true;

        
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(mouseScreenPos);
        Vector2 targetPosition = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
        

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        while (Vector2.Distance(rb.position, targetPosition) > 0.05f)
        {
            Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, dashSpeed * Time.deltaTime);
            rb.MovePosition(newPosition);
            yield return null;
        }

        rb.gravityScale = originalGravity;
        rb.linearVelocity = Vector2.zero;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}

