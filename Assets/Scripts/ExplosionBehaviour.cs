using UnityEngine;


/// <summary>
/// Handles the behavior of explosions in the game.
/// </summary>
public class ExplosionBehaviour : MonoBehaviour
{
    private EnemySpawner m_enemySpawner;
    private float m_explodeTime = 0.3f;

    private void Start()
    {
        m_enemySpawner = GameObject.Find("Enemy Spawner")?.GetComponent<EnemySpawner>();
        Invoke(nameof(Destroy), m_explodeTime);
    }

    /// <summary>
    /// Handles the event when an enemy is destroyed by the explosion.
    /// </summary>
    private void HandleEnemyDestroyed()
    {
        if (m_enemySpawner != null)
        {
            m_enemySpawner.EnemyDestroyed();

        }
    }

    /// <summary>
    /// Destroys the explosion object.
    /// </summary>
    private void Destroy()
    {
        Destroy(gameObject, 0.3f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HandleEnemyDestroyed();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject, 0.1f);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null && other.gameObject.CompareTag("SchoolFishes"))
        {
            //Destroy(other.gameObject, 0.1f);
            other.gameObject.SetActive(false);
        }
    }
}
