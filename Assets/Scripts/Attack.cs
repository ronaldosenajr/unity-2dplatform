using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
		public int attackDamage = 10;
		public Vector2 knockBack = Vector2.zero;

		void OnTriggerEnter2D(Collider2D other)
		{
			Damagable damageable = other.GetComponent<Damagable>();

			if (damageable != null)
			{
				Vector2 deliveredKnockBack = transform.parent.localScale.x > 0 ? knockBack : new Vector2(-knockBack.x, knockBack.y);
				damageable.Hit(attackDamage, deliveredKnockBack);
        Debug.Log(other.name + " was hit for " + attackDamage + " damage");
			}
		}
}
