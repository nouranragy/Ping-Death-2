

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
        if (!GameManager.Instance.isGameActive ) { return; }
       float newY = startPos.y + Mathf.PingPong(Time.time * moveSpeed, moveDistance) - (moveDistance / 2f);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

    }




    private void OnTriggerStay2D(Collider2D other)
    {
        
       OnTriggerEnter2D (other);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
       //Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
           // Cancel active player dash coroutine to allow physics launch
           Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            PlayerDashNewInput playerDash = other.GetComponent<PlayerDashNewInput>();

        if (playerRb != null)
            {
                
                Vector2 incomingVelocity = playerRb.linearVelocity.normalized;

                if (incomingVelocity.sqrMagnitude < 0.1f)
                {
                    incomingVelocity = (other.transform.position - transform.position).normalized;
                }

                
        
                Vector2 surfaceNormal = transform.up; 
                Vector2 reflectedDirection = Vector2.Reflect(incomingVelocity, transform.up).normalized;

               
                if (reflectedDirection.x < 0.2f)
                {
                    reflectedDirection.x = Mathf.Abs(reflectedDirection.x) + 0.5f; 
                    reflectedDirection.Normalize();

                
                if (playerDash != null)
                {
                    playerDash.CancelDash();
                }

                
                playerRb.linearVelocity = Vector2.zero;
                playerRb.linearVelocity = reflectedDirection.normalized * bounceForce;
            }


            
            
        }
    }

    
}
}
