using System;
using Enums;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawnController : MonoBehaviour
{ 
    [SerializeField] private int spawnedEnemy;
    [SerializeField] private int totalEnemy;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private SpellTypes enemyType;

    private void Start()
    {
        spawnedEnemy = 0;
    }

    private void Update()
    {
        if (spawnedEnemy<totalEnemy && !ObjectPoolingManager.Instance.IsPoolAllActive(enemyType + "Enemy"))
        {
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        GameObject enemy = ObjectPoolingManager.Instance.GetPooledObject(enemyType +"Enemy");
        if (enemy != null)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            enemy.transform.position = spawnPoint.position;
            enemy.SetActive(true);
            spawnedEnemy++;
            Debug.Log(spawnedEnemy);
        }
    }
}
