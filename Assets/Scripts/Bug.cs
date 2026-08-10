using UnityEngine;

public class Bug : MonoBehaviour
{

    protected void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            OnHitPlayer(other.gameObject);
        }
    }

    private void OnHitPlayer(GameObject player)
    {
        GameManager.Instance.LoseLife();
        Debug.Log("Player took damage");
    }
}
