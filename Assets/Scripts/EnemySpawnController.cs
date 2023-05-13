using System;
using System.Collections;
using Enums;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawnController : MonoBehaviour
{ 
    [SerializeField] private int killedEnemy;
    [SerializeField] private int totalEnemy;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private SpellTypes enemyType;
    [SerializeField] private int poolSize;
    [SerializeField] private GameObject poolParent;

    private void Start()
    {
        StartCoroutine(SpawningEnemies());
    }

    private void Update()
    {
        Debug.Log(killedEnemy);
    }

    private void SpawnEnemy()
    {
        GameObject enemy = ObjectPoolingManager.Instance.GetPooledObject(enemyType +"Enemy");
        if (enemy != null)
        {
            enemy.transform.parent = poolParent.transform;
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            enemy.transform.position = spawnPoint.position;
            //enemy.SetActive(true);
            Debug.Log("Enemy Spawned");
        }
    }

    IEnumerator SpawningEnemies()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < poolSize; i++)
        {
            SpawnEnemy();
        }
    }

    public void EnemyKilled()
    {
        killedEnemy++;
        if (killedEnemy < totalEnemy)
        {
            Debug.Log("Enemy can spawn");
            SpawnEnemy();
        }
    }
}
