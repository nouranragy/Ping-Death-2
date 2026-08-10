using UnityEngine;

public class Router : MonoBehaviour
{
   private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
           
            originalColor = spriteRenderer.color;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            SetRouterGreen();
        }
    }

    public void SetRouterGreen()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.green; 
        }
    }

   
    public void ResetRouterColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }
}
