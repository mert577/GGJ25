using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveEnemy : MonoBehaviour
{


    public int minSpawnAmount;

    public int maxSpawnAmount;

    public GameObject enemyPrefab;


    public float timeBetweenSpawns;

    public float spawnInternal;
    // Start is called before the first frame update
    void Start()
    {
        SpawningLoop();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawningLoop()
    {
        IEnumerator _SpawningLoop()
        {
            while (true)
            {
                SpawnBatchOfEnemies();
                yield return new WaitForSeconds(spawnInternal);
            }
        }

        StartCoroutine(_SpawningLoop());
    }

    public void SpawnBatchOfEnemies()
    {

        IEnumerator _SpawnBatchOfEnemies()
        {
            for (int i = 0; i < Random.Range(minSpawnAmount, maxSpawnAmount); i++)
            {
                //Spawn enemy
                GameObject enemy = Instantiate(enemyPrefab, transform.position +Random.insideUnitSphere*1.5f, Quaternion.identity);
                enemy.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle *2f, ForceMode2D.Impulse);
                yield return new WaitForSeconds(timeBetweenSpawns);
            }
        }

        StartCoroutine(_SpawnBatchOfEnemies());
    }
}
