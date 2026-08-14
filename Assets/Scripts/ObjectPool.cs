using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance  { get;  private set;}

    [Header ("Pool Settings")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize = 10;

    private Queue<GameObject> poolQueue = new Queue<GameObject>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            poolQueue.Enqueue(obj);
        }
    }

    public GameObject GetFromPool(Vector3 position, Quaternion rotation)
    {
        if (poolQueue.Count > 0)
        {
            GameObject obj = poolQueue.Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);
            return obj;
        }
        else
        {
           
            GameObject obj = Instantiate(prefab, position, rotation, transform);
            return obj;
        }
    }
     public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        poolQueue.Enqueue(obj);
    }

    
}
