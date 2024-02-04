using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Spawns and manages enemy instances in the game.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject m_enemyPrefab;

    [SerializeField]
    private Transform m_spawnPoint;

    [SerializeField]
    private int m_numberOfEnemies;

    [SerializeField]
    private float m_spawnTime;

    private AStar m_aStarAlgorithm;
    
    private bool isSpawning = false;

    private List<GameObject> m_enemiesList = new List<GameObject>();


    private void Start()
    {
        m_aStarAlgorithm = GameObject.Find("Game").GetComponent<AStar>();
        StartCoroutine(SpawnEnemyRoutine());
    }

    private void Update()
    {
        if (m_aStarAlgorithm != null && m_aStarAlgorithm.m_isTailsGridCreated && m_numberOfEnemies > 0)
        {
            StartCoroutine(SpawnEnemyRoutine());
        }
    }

    /// <summary>
    /// Spawns enemies at regular intervals.
    /// </summary>
    /// <returns>Coroutine enumerator.</returns>
    private IEnumerator SpawnEnemyRoutine()
    {
        if (isSpawning)
        {
            yield break;
        }

        isSpawning = true;

        while (m_aStarAlgorithm.m_isTailsGridCreated && m_numberOfEnemies > 0)
        {
            yield return new WaitForSeconds(m_spawnTime);
            SpawnEnemy();
        }

        isSpawning = false;
    }

    /// <summary>
    /// Spawns a new enemy at the specified spawn point.
    /// </summary>
    private void SpawnEnemy()
    {
        GameObject enemy = Instantiate(m_enemyPrefab, m_spawnPoint.position, Quaternion.identity);
        enemy.transform.SetParent(gameObject.transform, false);
        m_enemiesList.Add(enemy);
        m_numberOfEnemies--;
    }

    /// <summary>
    /// Increases the count of available enemies when an enemy is destroyed.
    /// </summary>
    public void EnemyDestroyed()
    {
        m_numberOfEnemies++;
    }
}
