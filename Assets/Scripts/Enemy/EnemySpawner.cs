using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnableEnemies;
    [SerializeField] private Transform[] spawnLocations;
    [SerializeField] private int enemiesPerWave;
    [SerializeField] private int numberOfWaves;
    [SerializeField] private float timeBetweenWaves;
    [SerializeField] private bool canSpawnEnemies = true;
    [SerializeField] private bool spawnedEnemyWillPursue = true;

    private float timeToNextWave = 0f;
    private int currentWave = 1;

    private void Update()
    {
        if (timeToNextWave <= 0f && canSpawnEnemies && currentWave <= numberOfWaves)
            SpawnWave();
        else if (timeToNextWave > 0f)
            timeToNextWave -= Time.deltaTime;
    }

    private void SpawnWave()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            Transform location = spawnLocations[Random.Range(0, spawnLocations.Length - 1)].transform;
            GameObject enemy = Instantiate(spawnableEnemies[Random.Range(0, spawnableEnemies.Length - 1)], location.position, Quaternion.identity, location);
            if (enemy.TryGetComponent(out Enemy e))
            {
                e.BasePosition = location;
                e.WillPursue = spawnedEnemyWillPursue;
            }
            else
            {
                Enemy spawnedEnemy = enemy.GetComponentInChildren<Enemy>();
                spawnedEnemy.BasePosition = location;
                spawnedEnemy.WillPursue = spawnedEnemyWillPursue;
            }
        }
        currentWave++;
        if (currentWave > numberOfWaves)
            canSpawnEnemies = false;
        else
            timeToNextWave = timeBetweenWaves;
    }
}
