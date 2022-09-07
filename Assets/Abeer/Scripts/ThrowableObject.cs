using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.enemyTag))
        {
            Destroy(gameObject);
        }
    }
}
