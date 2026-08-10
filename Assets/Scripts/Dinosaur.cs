using UnityEngine;
using System.Collections;

public class Dinosaur : MonoBehaviour
{
    [Header ("Movement Settings")]
     [SerializeField] private float speed = 3f;

     [Header ("Timer Settings")]
      [SerializeField] private float timeToSaveRouter = 5f;
      private float currentTimer;
      private Transform targetRouter;
      private bool isAttacking = false;
      private bool isTimerRunning = false;

      public void Initialize(Transform routerTransform)
    {
        targetRouter = routerTransform;
        isAttacking = true;
        isTimerRunning = false;
        currentTimer = timeToSaveRouter;
    }
    

    // Update is called once per frame
   private  void Update()
    {
        if(!isAttacking || targetRouter == null) return;
        if (!isTimerRunning)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetRouter.position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetRouter.position) < 0.2f)
            {
                isTimerRunning = true;
                StartCoroutine(StartAttackCountdown());
            }
        }
    }

    private IEnumerator StartAttackCountdown()
    {
        while (currentTimer > 0)
        {
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

       
        ObjectPool.Instance.ReturnToPool(gameObject);
    }

    private void ResetLevelProgress()
    {
      
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = Vector3.zero;
        }

       
        Router[] allRouters = FindObjectsOfType<Router>();
        foreach (Router router in allRouters)
        {
            router.ResetRouterColor();
        }

      
        DefeatDinosaur();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.CompareTag("Player"))
        {
            DefeatDinosaur();
        }
    }
}
