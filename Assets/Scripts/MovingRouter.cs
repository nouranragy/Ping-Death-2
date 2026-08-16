using UnityEngine;

public class MovingRouter : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Vector2 moveOffset = new Vector2(3f,0f);
    [SerializeField] private float speed = 2f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    
    void Update()
    {
        if (!GameManager.Instance.isGameActive) { return; }
        // Smooth linear back-and-forth movement using Mathf.PingPong
        float factor = Mathf.PingPong(Time.time * speed, 1f);
        transform.position = Vector3.Lerp(startPos, startPos + (Vector3)moveOffset, factor);

    }
    // Visualization in Editor to see movement path
    private void OnDrawGizmosSelected()
    {
        Vector3 from = Application.isPlaying ? startPos : transform.position;
        Vector3 to = from + (Vector3)moveOffset;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(from, to);
        Gizmos.DrawWireSphere(from, 0.2f);
        Gizmos.DrawWireSphere(to, 0.2f);
    }
}
