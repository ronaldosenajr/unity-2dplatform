using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnEnable()
    {
        if (player)
            player.GetComponent<Damagable>().damageableDeath.AddListener(OnPlayerDeath);
    }

    void OnDisable()
    {
        if (player)
            player.GetComponent<Damagable>().damageableDeath.RemoveListener(OnPlayerDeath);
    }

    void OnPlayerDeath()
    {
        Debug.Log("Player died");
        StartCoroutine(RestartGame());
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("GameplayScene");
    }
}
