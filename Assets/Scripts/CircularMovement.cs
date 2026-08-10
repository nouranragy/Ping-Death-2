using UnityEngine;

public class CircularMovement : MonoBehaviour
{
    
    [SerializeField] private float radius = 2f;
    [SerializeField] private float speed = 2f;

    private Vector3 centerPos;
    private float angle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        centerPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        angle += speed * Time.deltaTime;
        float x = centerPos.x + Mathf.Cos(angle) * radius;
        float y = centerPos.y + Mathf.Sin(angle) * radius;

        transform.position = new Vector3(x, y, centerPos.z);
    }
}
