using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Handles the behavior of the player character in the game.
/// </summary>
public class PlayerBehaviour : GamePerson
{
    private MovementType m_movementType;

    [SerializeField]
    private float m_maxAngularVelocity;

    [SerializeField]
    private float m_maxDepenetrationVelocity;

    [SerializeField]
    private float m_maxLinearVelocity;

    [SerializeField]
    private GameObject m_minePrefab;

    private string m_healthKey = "HealthKey";

    private int m_health;

    private string m_movementKey = "MovementKey";

    private Rigidbody rb;

    [SerializeField]
    private UnityEvent m_restartSceneEvent;

    [SerializeField]
    private UnityEvent m_gameOverEvent;

    [SerializeField]
    private UnityEvent m_gameWinEvent;

    [SerializeField]
    private LifeCounterBehaviour m_lifeCounterBehaviour;

    private TailCenterCalculate m_tailCenterCalculate;

    private HashSet<Vector3> m_minesPositions = new HashSet<Vector3>();

    private enum MovementType
    {
        VelocityMode,
        ForceMode
    }

    public bool IsDead { get; set; }

    [SerializeField]
    private AudioClip m_diveSound;
    private string _musicVolumeKey = "MusicVolumeKey";

    #region Private Methods

    private void Start()
    {
        if (PlayerPrefs.HasKey(m_movementKey))
        {
            int moveType = PlayerPrefs.GetInt(m_movementKey, -1);
            switch (moveType)
            {
                case 0:
                    {
                        m_movementType = MovementType.VelocityMode;
                        break;
                    }
                case 1:
                    {
                        m_movementType = MovementType.ForceMode;
                        break;
                    }
                default:
                    {
                        m_movementType = MovementType.VelocityMode;
                        break;
                    }
            }
        }
        else
            m_movementType = MovementType.VelocityMode;

        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.maxAngularVelocity = m_maxAngularVelocity;
            rb.maxDepenetrationVelocity = m_maxDepenetrationVelocity;
            rb.maxLinearVelocity = m_maxLinearVelocity;
        }

        m_tailCenterCalculate = gameObject.AddComponent<TailCenterCalculate>();

        if (PlayerPrefs.HasKey(m_healthKey))
        {
            m_health = PlayerPrefs.GetInt(m_healthKey, 3);
        }

        IsDead = true;

        if (m_diveSound != null)
        {
            AudioSource.PlayClipAtPoint(m_diveSound, transform.position, PlayerPrefs.GetFloat(_musicVolumeKey, 0.2f));
        }
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            if (m_movementType == MovementType.ForceMode)
            {
                HandleForceModeInput();
            }
        }
        else
            Debug.LogError("RigidBody component not found!");
    }

    private void Update()
    {
        if (rb != null)
        {
            if (m_movementType == MovementType.VelocityMode)
            {
                HandleVelocityModeInput();
            }
        }
        else
            Debug.LogError("RigidBody component not found!");

        HandleMinePlacement();
    }

    private void OnDestroy()
    {
        if (IsDead)
        {
            HandlePlayerDeath();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Prize"))
        {
            IsDead = false;
            m_gameWinEvent.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bonus"))
        {
            HandleBonusLife(other.gameObject);
        }
    }

    private void BonusLife()
    {
        m_health++;
        PlayerPrefs.SetInt(m_healthKey, m_health);
        m_lifeCounterBehaviour.AddLifeIcon();
    }

    private Vector3 CenteringMinePosition(Vector3 pos)
    {
        Vector2 centerVec2 = m_tailCenterCalculate.CenteringPosition(pos);
        return new Vector3(centerVec2.x, 0.8f, centerVec2.y);
    }

    private void Movement(Vector3 moveDir, MovementType movementType)
    {
        switch (movementType)
        {
            case MovementType.ForceMode:
                {
                    rb.AddForce(moveDir * MovementSpeed);
                    break;
                }
            case MovementType.VelocityMode:
                {
                    rb.velocity += moveDir;
                    rb.velocity = rb.velocity.normalized * MovementSpeed;
                    break;
                }
        }

        RotateMovement(moveDir);
    }

    private void RotateMovement(Vector3 direction)
    {
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, RotationSpeed);
    }

    private void HandleForceModeInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Movement(Vector3.right, m_movementType);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Movement(Vector3.left, m_movementType);
        }
        if (Input.GetKey(KeyCode.W))
        {
            Movement(Vector3.back, m_movementType);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Movement(Vector3.forward, m_movementType);
        }
    }

    private void HandleVelocityModeInput()
    {
        rb.velocity = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
        {
            Movement(Vector3.right, m_movementType);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Movement(Vector3.left, m_movementType);
        }
        if (Input.GetKey(KeyCode.W))
        {
            Movement(Vector3.back, m_movementType);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Movement(Vector3.forward, m_movementType);
        }
    }

    private void HandleMinePlacement()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_tailCenterCalculate != null && !m_minesPositions.Contains(CenteringMinePosition(transform.position)))
            {
                Vector3 minePrefab = Instantiate(m_minePrefab, CenteringMinePosition(transform.position), Quaternion.identity).transform.position;
                m_minesPositions.Add(minePrefab);
            }
        }
    }

    private void HandlePlayerDeath()
    {
        m_health--;

        if (m_health > 0)
        {
            PlayerPrefs.SetInt(m_healthKey, m_health);
            m_restartSceneEvent.Invoke();
        }
        else
        {
            PlayerPrefs.SetInt(m_healthKey, m_health);
            m_gameOverEvent.Invoke();
        }
    }

    private void HandleBonusLife(GameObject bonusObject)
    {
        BonusLife();
        Destroy(bonusObject);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Called when a mine is exploded, removing its position from the HashSet.
    /// </summary>
    /// <param name="minePos">The position of the exploded mine.</param>
    public void MineIsExploded(MineInfo minePos)
    {
        m_minesPositions.Remove(minePos.MinePosition);
    }

    #endregion
}
