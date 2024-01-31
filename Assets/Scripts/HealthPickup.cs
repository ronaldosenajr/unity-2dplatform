using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthRestore = 20;
    public Vector3 spinRotationSpeed = new Vector3(0, 180, 0);

    AudioSource pickupSource;

    void Awake()
    {
        pickupSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Damagable damageable = other.GetComponent<Damagable>();

        if (damageable)
        {
            bool wasHealed = damageable.Heal(healthRestore);
            if (wasHealed)
                if (pickupSource)
                    AudioSource.PlayClipAtPoint(pickupSource.clip, transform.position, pickupSource.volume);
                Destroy(gameObject);
        }
    }

    void Update()
    {
        transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
    }
}
