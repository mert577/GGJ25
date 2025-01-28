using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{


    public static WaveSpawner instance;

    public EnemyCostData[] easyEnemyPool;
    public EnemyCostData[] mediumEnemyPool;
    public EnemyCostData[] hardEnemyPool;

    public EnemyCostData[] endgameEnemyPool;



    public List<int> waveCurrencies;

    public List<int> waveNumberToPoolMap;


    public Transform playerPosition;

    public float enemySpawnRadius;


    public int aliveEnemies = 0;

    public bool isSpawning = false;


    public float timeToCheckForEnemies = 1f;
    float enemyCheckTimer = 0f;



    public int currentWave = 0;

    public List<EnemyCostData[]> enemyPools = new List<EnemyCostData[]>();

    public bool gameStarted = false;


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

        enemyPools.Add(easyEnemyPool);
        enemyPools.Add(mediumEnemyPool);
        enemyPools.Add(hardEnemyPool);
        enemyPools.Add(endgameEnemyPool);
    }


    // Start is called before the first frame update


    public void SpawnFirstWave()
    {
        GameObject[] enemiesToSpawn = GenerateEnemyList(enemyPools[waveNumberToPoolMap[currentWave]], waveCurrencies[0]);
        SpawnEnemies(enemiesToSpawn);
        currentWave++;
        gameStarted = true;
    }


    void Start()
    {
        //spawn first wave
    }

    // Update is called once per frame
    void Update()
    {

        if (!gameStarted)
        {
            return;
        }

        enemyCheckTimer -= Time.deltaTime;
        if (enemyCheckTimer <= 0)
        {
            enemyCheckTimer = timeToCheckForEnemies;
            //            Debug.Log("Checking for enemies");
            if (aliveEnemies == 0 && !isSpawning)
            {
                Debug.Log("Spawning new wave");

                EnemyCostData[] currentPool;
                int currency;
                if (waveNumberToPoolMap.Count <= currentWave)
                {
                    Debug.LogError("Wave number to pool map does not have enough entries for wave " + currentWave);
                    currentPool = endgameEnemyPool;
                    currency = waveCurrencies[waveCurrencies.Count - 1] + (currentWave - waveCurrencies.Count + 1) * 20;

                }
                else
                {
                    currentPool = enemyPools[waveNumberToPoolMap[currentWave]];
                    currency = waveCurrencies[currentWave];
                }
                GameObject[] enemiesToSpawn = GenerateEnemyList(currentPool, currency);
                if (enemiesToSpawn.Length > 0)
                {
                    currentWave++;
                    SpawnEnemies(enemiesToSpawn);
                }
                else
                {
                    Debug.Log("No enemies to spawn");
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
