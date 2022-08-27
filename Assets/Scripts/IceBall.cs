using UnityEngine;

public class IceBall : MonoBehaviour
{
    [HideInInspector]
    public Vector2 moveDir;

    [SerializeField] Rigidbody2D RB;
    //[SerializeField] GameObject impactFX;

    [Space]
    [SerializeField] float speed;

    void Update() => RB.velocity = moveDir * speed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.enemyTag))
        {


            Destroy(gameObject); 
        }
    }

    private void OnBecameInvisible() => Destroy(gameObject);
}
