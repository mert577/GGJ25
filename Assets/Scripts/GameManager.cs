using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{


    public static GameManager instance;

    public int playerScore;

    public bool isGameActive = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnDeath()
    {
        Debug.Log("Player has died");
    }


    public void AddScore(int amount)
    {
        playerScore += amount;
    }

    public void GameStart()
    {
        UIManager.instance.EnableInGamePanel();
        UIManager.instance.DisableTitlePanel();
        WaveSpawner.instance.SpawnFirstWave();
        isGameActive = true;
    }
    public void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
