using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector2 moveSpeed = new Vector2(3, 0);
    public Vector2 knockback = new Vector2(0, 0);
    public int damage = 10;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = new Vector2(moveSpeed.x * transform.localScale.x, moveSpeed.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damagable damageable = collision.GetComponent<Damagable>();
        if (damageable)
        {
            Vector2 deliveredKnockback = transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);

            bool gotHit = damageable.Hit(damage, deliveredKnockback);
            if (gotHit)
            {
                Destroy(gameObject);
            }
        }
    }

}
