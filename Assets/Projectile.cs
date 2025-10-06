using UnityEngine;


public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private float lifetime = 5f;
    private ShipController poolOwner;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 direction, float speed, ShipController owner)
    {
        poolOwner = owner;
        rb.velocity = Vector2.zero;

        rb.velocity = direction * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Invoke(nameof(ReturnToPool), lifetime);
    }

    void ReturnToPool()
    {
        CancelInvoke(nameof(ReturnToPool));

        poolOwner.ReturnToPool(this);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ReturnToPool();
            GameManager.instance.Lifelost();
        }

        
    }
}
