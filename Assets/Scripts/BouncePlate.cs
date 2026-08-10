

using UnityEngine;

public class BouncePlate : MonoBehaviour
{
    [Header("Bounce Settings")]
    [SerializeField] private Vector2 bounceDirection = Vector2.up; // launch direction
    [SerializeField] private float bounceForce = 15f; // force applied to the bounce
    [Header("Movement Settings")]
    [SerializeField] private float moveDistance = 2f; // distance range for vertical movement
    [SerializeField] private float moveSpeed = 2f;  //speed of vertical movement
    private Vector3 startPos;
    private void Start()
    {
        startPos =  transform.position;
    }
    private void Update()
    {
        // move the plate up and down using pingpong
       float newY = startPos.y + Mathf.PingPong(Time.time * moveSpeed, moveDistance) - (moveDistance / 2f);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
       //Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
           // Cancel active player dash coroutine to allow physics launch
            PlayerDashNewInput playerDash = other.GetComponent<PlayerDashNewInput>();
            if (playerDash != null)
            {
                playerDash.CancelDash();
            }

            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // Apply direct impulse velocity along the specified bounce direction
                playerRb.linearVelocity = bounceDirection.normalized * bounceForce;
            }
        }
    }

    
}
