using UnityEngine;

public class MovingBug : Bug
{
    private Animator anim;
    [Header("Movement Settings")]
    [SerializeField] private Transform[] waypoints; 
    [SerializeField] private float moveSpeed = 3f;   

    private int currentWaypointIndex = 0;

    void Start()  
    {
        anim = GetComponent<Animator>(); 
        if  (GameManager.Instance.isGameActive) anim.SetBool("isMoving", true);
       
    }
    
    private void FixedUpdate()
    {
        if (!GameManager.Instance.isGameActive) { return; }

        MoveBetweenPoints();
    }

    private void MoveBetweenPoints()
    {

        Transform targetPoint = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

        
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}