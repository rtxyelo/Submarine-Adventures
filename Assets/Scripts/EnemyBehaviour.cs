using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Controls the behavior of an enemy game character using the A* algorithm for pathfinding.
/// </summary>
public class EnemyBehaviour : GamePerson
{
    private AStar m_aStarAlgorithm;

    private Transform m_playerPositionTransform;
    
    private Vector3 m_playerPosition;

    private TailCenterCalculate m_tailCenterCalculate;

    private List<Tail> m_path;

    private bool m_isFirstPathPoint = true;


    #region Private Methods

    private void Start()
    {
        m_playerPositionTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        m_playerPosition = m_playerPositionTransform.position;

        m_tailCenterCalculate = gameObject.AddComponent<TailCenterCalculate>();

        m_aStarAlgorithm = GameObject.Find("Game").GetComponent<AStar>();

        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    private void Update()
    {
        if (m_playerPosition != null)
            m_playerPosition = m_playerPositionTransform.position;

        if (m_aStarAlgorithm.m_isTailsGridCreated)
        {
            if (m_isFirstPathPoint)
            {
                m_path = m_aStarAlgorithm.GetPath(transform.position, m_playerPosition);
                m_isFirstPathPoint = false;
            }

            MoveEnemy(m_path[0].worldPosition);

            if (CenteringEnemyPosition(transform.position) == m_path[0].worldPosition)
            {
                m_path = m_aStarAlgorithm.GetPath(transform.position, m_playerPosition);
            }
        }
    }

    /// <summary>
    /// Centers the enemy position using TailCenterCalculate.
    /// </summary>
    /// <param name="pos">The current position of the enemy.</param>
    /// <returns>The centered position of the enemy.</returns>
    private Vector3 CenteringEnemyPosition(Vector3 pos)
    {
        Vector2 centerVec2 = m_tailCenterCalculate.CenteringPosition(pos);
        return new Vector3(centerVec2.x, 0f, centerVec2.y);
    }

    /// <summary>
    /// Rotates the enemy character towards the specified direction.
    /// </summary>
    /// <param name="dir">The target direction to rotate towards.</param>
    private void RotateMovement(Vector3 dir)
    {
        if (transform.position != dir)
        {
            Vector3 direction = (dir - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));

            Quaternion finalRotation = lookRotation * Quaternion.Euler(0f, 90f, 0f);

            transform.rotation = Quaternion.Lerp(transform.rotation, finalRotation, RotationSpeed);
        }
    }

    /// <summary>
    /// Moves the enemy towards the specified point and adjusts its rotation.
    /// </summary>
    /// <param name="nextPoint">The target point to move towards.</param>
    private void MoveEnemy(Vector3 nextPoint)
    {
        nextPoint.y += 0.5f;
        transform.position = Vector3.MoveTowards(transform.position, nextPoint, MovementSpeed * Time.deltaTime);

        RotateMovement(nextPoint);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(collision.gameObject, 0.1f);
        }
    }

    #endregion

}
