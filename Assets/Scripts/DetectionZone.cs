using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectionZone : MonoBehaviour
{
    public UnityEvent noCollidersRemaining;
    public List<Collider2D> detectedColliders = new List<Collider2D>();

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            Damagable playerDamageable = player.GetComponent<Damagable>();
            if (playerDamageable && playerDamageable.Health <= 0)
            {
                detectedColliders.Clear();
                return;
            }
        }
        detectedColliders.Add(collision);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        detectedColliders.Remove(collision);
        if (detectedColliders.Count <= 0)
        {
            noCollidersRemaining.Invoke();
        }
    }
}
