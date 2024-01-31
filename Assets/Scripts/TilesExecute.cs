using System;
using UnityEngine;

/// <summary>
/// Manages the execution and manipulation of tiles in the game field.
/// </summary>
public class TilesExecute : MonoBehaviour
{
    [SerializeField] private GameObject m_pathTailPrefab;
    [SerializeField] private GameObject m_barrierTailPrefab;
    [SerializeField] private GameField _gameField;

    private Vector2Int m_gameFieldSize;
    [HideInInspector] public GameObject[] m_pathTails;
    [HideInInspector] public GameObject[] m_barrierTails;
    [HideInInspector] public GameObject[] m_tails;
    [HideInInspector] public GameObject[,] m_tilesTwoDim;
    [HideInInspector] public bool m_tailsAreCalculate = false;

    private static int GLOBAL_TILES_COUNT = 200;
    private static int PATH_TILES_COUNT = 149;
    private static int BARRIER_TILES_COUNT = 51;
    private int m_barrierTilesCount = 0;

    #region Private Methods

    private void Start()
    {
        m_gameFieldSize = new Vector2Int(_gameField.m_gameFieldSize.x, _gameField.m_gameFieldSize.y);
        m_tilesTwoDim = new GameObject[_gameField.m_gameFieldSize.y, _gameField.m_gameFieldSize.x];
        SetGameTails();
    }

    /// <summary>
    /// Initializes the game field by instance of path/barrier elements.
    /// </summary>
    private void SetGameTails()
    {
        Vector2 offset = new Vector2((m_gameFieldSize.x - 1) * 0.5f, (m_gameFieldSize.y - 1) * 0.5f);

        m_tails = new GameObject[m_gameFieldSize.x * m_gameFieldSize.y];

        for (int y = 0; y < m_gameFieldSize.y; y++)
        {
            for (int x = 0; x < m_gameFieldSize.x; x++)
            {
                GameObject tile = m_tails[y * m_gameFieldSize.x + x] = m_tilesTwoDim[y, x] = Instantiate(m_pathTailPrefab);
                tile.transform.SetParent(gameObject.transform, false);
                tile.transform.localPosition = new Vector3(x - offset.x, 0f, y - offset.y);
            }
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Resets the specified path tile to a barrier tile and updates the count of barrier tiles.
    /// </summary>
    /// <param name="go">The GameObject representing the path tile to be reset.</param>
    public void ResetPathTails(GameObject go)
    {
        for (int y = 0; y < m_gameFieldSize.y; y++)
        {
            for (int x = 0; x < m_gameFieldSize.x; x++)
            {
                if (m_tails[y * m_gameFieldSize.x + x] == go)
                {
                    Vector3 pos = m_tails[y * m_gameFieldSize.x + x].transform.localPosition;
                    Destroy(m_tails[y * m_gameFieldSize.x + x]);
                    GameObject tile = m_tails[y * m_gameFieldSize.x + x] = m_tilesTwoDim[y, x] = Instantiate(m_barrierTailPrefab, pos, Quaternion.identity);
                    tile.transform.SetParent(gameObject.transform, false);
                }
            }
        }
        m_barrierTilesCount++;
        if (m_barrierTilesCount == BARRIER_TILES_COUNT)
        {
            m_tailsAreCalculate = true;
        }
    }

    /// <summary>
    /// Removes all tail prefabs from the game field.
    /// </summary>
    public void RemoveTailPrefabsFromGameField()
    {
        foreach (var tile in m_tails)
        {
            Destroy(tile.gameObject);
        }
        Array.Clear(m_tails, 0, m_tails.Length);
    }

    /// <summary>
    /// Recalculates the path and barrier tile arrays based on the current state of the game field. (FOR ML. NO ACTUAL)
    /// </summary>
    public void RecalculateTiles()
    {
        m_pathTails = new GameObject[PATH_TILES_COUNT];
        m_barrierTails = new GameObject[BARRIER_TILES_COUNT];

        int pathInd = 0;
        int barrierInd = 0;

        for (int i = 0; i < GLOBAL_TILES_COUNT; i++)
        {
            if (pathInd < PATH_TILES_COUNT && m_tails[i].CompareTag("Path"))
            {
                m_pathTails[pathInd] = m_tails[i];
                pathInd += 1;
            }
            else if (barrierInd < BARRIER_TILES_COUNT && m_tails[i].CompareTag("Barrier"))
            {
                m_barrierTails[barrierInd] = m_tails[i];
                barrierInd += 1;
            }
        }
    }

    #endregion
}
