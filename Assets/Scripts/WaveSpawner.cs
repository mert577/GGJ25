using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{


    public static WaveSpawner instance;

    public EnemyCostData[] easyEnemyPool;
    public EnemyCostData[] mediumEnemyPool;
    public EnemyCostData[] hardEnemyPool;



    public List<int> waveCurrencies;


    public Transform playerPosition;

    public float enemySpawnRadius;


    public int aliveEnemies = 0;

    public bool isSpawning = false;


    public float timeToCheckForEnemies = 1f;
    float enemyCheckTimer = 0f;



    public int currentWave = 0;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        playerPosition = GameObject.Find("Player").transform;
    }


    // Start is called before the first frame update
    void Start()
    {
        //spawn first wave

        GameObject[] enemiesToSpawn = GenerateEnemyList(easyEnemyPool, waveCurrencies[0]);
        SpawnEnemies(enemiesToSpawn);
        currentWave++;
    }

    // Update is called once per frame
    void Update()
    {

        enemyCheckTimer -= Time.deltaTime;
        if (enemyCheckTimer <= 0)
        {
            enemyCheckTimer = timeToCheckForEnemies;
            Debug.Log("Checking for enemies");
            if (aliveEnemies == 0 && !isSpawning)
            {
                Debug.Log("Spawning new wave");
                if (waveCurrencies.Count > currentWave)
                {
                    GameObject[] enemiesToSpawn = GenerateEnemyList(easyEnemyPool, waveCurrencies[currentWave]);
                    if (enemiesToSpawn.Length > 0)
                    {
                        currentWave++;
                        SpawnEnemies(enemiesToSpawn);
                    }
                    else{
                        Debug.Log("No enemies to spawn");
                    }
                }
            }
        }

    }


    void SpawnEnemies(GameObject[] enemiesToSpawn)
    {

        IEnumerator SpawnEnemiesCoroutine(GameObject[] enemiesToSpawn)
        {
            isSpawning = true;
            foreach (GameObject enemy in enemiesToSpawn)
            {
                Vector3 spawnPosition = playerPosition.position + (Vector3)Random.insideUnitCircle.normalized * enemySpawnRadius;
                spawnPosition.z = 0;
                Instantiate(enemy, spawnPosition, Quaternion.identity);
                aliveEnemies++;
                yield return new WaitForSeconds(.3f);
            }
            isSpawning = false;
        }
        StartCoroutine(SpawnEnemiesCoroutine(enemiesToSpawn));
    }

    GameObject[] GenerateEnemyList(EnemyCostData[] enemyPool, int currency)
    {
        List<GameObject> enemies = new List<GameObject>();
        int currencyLeft = currency;
        int failedAttempts = 0;
        while (currencyLeft > 0 && failedAttempts < enemyPool.Length)
        {
            int randomIndex = Random.Range(0, enemyPool.Length);
            if (currencyLeft - enemyPool[randomIndex].cost >= 0)
            {
                currencyLeft -= enemyPool[randomIndex].cost;
                enemies.Add(enemyPool[randomIndex].enemyPrefab);
                failedAttempts = 0;
            }
            else
            {
                failedAttempts++;
            }
        }
        return enemies.ToArray();
    }

}
