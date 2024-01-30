using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
		public GameObject damageTextPrefab;
		public GameObject healthTextPrefab;

		public Canvas gameCanvas;

		void Awake()
		{
			gameCanvas = FindObjectOfType<Canvas>();
		}

		void OnEnable()
		{
			CharacterEvents.characterDamaged += CharacterTookDamage;
			CharacterEvents.characterHealed += CharacterHealed;
		}

		void OnDisable()
		{
			CharacterEvents.characterDamaged -= CharacterTookDamage;
			CharacterEvents.characterHealed -= CharacterHealed;
		}

	 	public void CharacterTookDamage(GameObject character, int damage)
		{
			Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
			TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();
			tmpText.text = damage.ToString();
		}

		public void CharacterHealed(GameObject character, int amount)
		{
			Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
			TMP_Text tmpText = Instantiate(healthTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();
			tmpText.text = amount.ToString();
		}
}
