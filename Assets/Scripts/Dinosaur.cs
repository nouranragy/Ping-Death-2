using UnityEngine;
using System.Collections;

public class Dinosaur : MonoBehaviour
{
    [Header ("Movement Settings")]
     [SerializeField] private float speed = 3f;
     [SerializeField] private float scareDistance = 2.5f; 
    [SerializeField] private float fleeSpeed = 5f;

     [Header ("Timer Settings")]
      [SerializeField] private float timeToSaveRouter = 5f;
      private float currentTimer;
      private Node targetNode;
      private Transform playerTransform;
      private bool isAttacking = false;
      private bool isTimerRunning = false;
      private bool isFleeing = false;
    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

      public void Initialize(Node nodeToAttack)
    {
        targetNode = nodeToAttack;
        isAttacking = true;
         targetNode.isUnderAttack = true;
        isTimerRunning = false;
        isFleeing = false;
        currentTimer = timeToSaveRouter;
    }
    

    // Update is called once per frame
   private  void Update()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null) playerTransform = player.transform;
        }

        
        if (isFleeing)
        {
            FleeFromPlayer();
            return;
        }

        if (!isAttacking || targetNode == null) return;

        
        if (playerTransform != null && Vector2.Distance(transform.position, playerTransform.position) < scareDistance)
        {
            ScareAndRunAway();
            return;
        }

         if (targetNode.stateMachine.CurrentState != targetNode.connectedState)
        {
            ScareAndRunAway();
            return;
        }

        if (!isTimerRunning)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetNode.transform.position, speed * Time.deltaTime);
            
            if (Vector3.Distance(transform.position, targetNode.transform.position) < 0.2f)
            {
                isTimerRunning = true;
                StartCoroutine(StartAttackCountdown());
            }
        }
    }
    private void ScareAndRunAway()
    {
        StopAllCoroutines();
        isTimerRunning = false;
        isAttacking = false;
        isFleeing = true;
    }

    private void FleeFromPlayer()
    {
        if (playerTransform != null)
        {
            
            Vector3 fleeDirection = (transform.position - playerTransform.position).normalized;
            transform.position += fleeDirection * fleeSpeed * Time.deltaTime;
            if (Vector3.Distance(transform.position, playerTransform.position) > 10f)
            {
                ReturnToPool();
            }
        }
        else
        {
            ReturnToPool();
        }
    }

    private IEnumerator StartAttackCountdown()
    {
      while (currentTimer > 0)
        {
        
            if (playerTransform != null && Vector2.Distance(transform.position, playerTransform.position) < scareDistance)
            {
                ScareAndRunAway();
                yield break;
            }

            currentTimer -= Time.deltaTime;
            yield return null;
        }

        
        ResetLevelProgress();
    }

    public void DefeatDinosaur()
    {
        StopAllCoroutines();
        isTimerRunning = false;
        isAttacking = false;
        targetNode = null;
        if (ObjectPool.Instance != null)
        {
            ObjectPool.Instance.ReturnToPool(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }

       
       
    }

    private void ResetLevelProgress()
    {
      Debug.Log("Dinosaur ruined the Node! Disconnecting this node only...");

    
    if (ChainManager.Instance != null && targetNode != null)
    {
        ChainManager.Instance.DisconnectSingleNode(targetNode);
    }
        
      
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        StopAllCoroutines();
        if(targetNode != null)
        {
            targetNode.isUnderAttack=false;
            targetNode = null;
        }
        isTimerRunning = false;
        isAttacking = false;
        isFleeing = false;
        

        if (ObjectPool.Instance != null)
        {
            ObjectPool.Instance.ReturnToPool(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

   
}
