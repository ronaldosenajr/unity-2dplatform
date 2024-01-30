using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthText : MonoBehaviour
{
	public Vector3 moveSpeed = new Vector3(0, 75, 0);
	public float TimeToFade = 1f;
	RectTransform textTransform;
	TextMeshProUGUI textMeshPro;
	private float timeElapsed = 0f;
	private Color startColor;

	void Awake()
	{
		textTransform = GetComponent<RectTransform>();
		textMeshPro = GetComponent<TextMeshProUGUI>();
		startColor = textMeshPro.color;
	}

	void Update()
	{
		textTransform.position += moveSpeed * Time.deltaTime;
		timeElapsed += Time.deltaTime;
		float fadeAlpha = startColor.a * (1 - (timeElapsed / TimeToFade));
		textMeshPro.color = new Color(startColor.r, startColor.g, startColor.b, fadeAlpha);
		if (timeElapsed >= TimeToFade)
		{
			Destroy(gameObject);
		}
	}
}
