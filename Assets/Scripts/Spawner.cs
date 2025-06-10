using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject speedboostPrefab;
    public GameObject shieldPrefab;
    public Transform player;
    private float spawnRadius = 15f;
    public float minDistance = 5f;
    private bool spawnShieldNext = true;

    void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            SpawnEnemy();
        }

        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnBoost());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            float waitTime = Random.Range(3f, 10f);
            yield return new WaitForSeconds(waitTime);
            SpawnEnemy();
        }
    }

    IEnumerator SpawnBoost()
    {
        while (true)
        {
            float waitTime = Random.Range(45f, 60f);
            yield return new WaitForSeconds(waitTime);
            Vector3 spawnPos = GetSpawnPos();
            GameObject prefabToSpawn = spawnShieldNext ? shieldPrefab : speedboostPrefab;
            Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
            spawnShieldNext = !spawnShieldNext;
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPos = GetSpawnPos();
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }

    Vector3 GetSpawnPos()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float distance = Random.Range(minDistance, spawnRadius);
        Vector3 spawnPos = player.position + new Vector3(randomDirection.x, 0f, randomDirection.y) * distance;
        return spawnPos;
    }
}