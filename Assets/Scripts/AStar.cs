using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Represents a node in the A* algorithm used for pathfinding.
/// </summary>
public class Tail
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;
    public int gCost = 0;
    public int hCost = 0;
    public Tail parent;

    /// <summary>
    /// Initializes a new instance of the <see cref="Tail"/> class.
    /// </summary>
    /// <param name="_walkable">Whether the tail is walkable or not.</param>
    /// <param name="_worldPos">The world position of the tail.</param>
    /// <param name="_gridX">The X coordinate of the tail in the grid.</param>
    /// <param name="_gridY">The Y coordinate of the tail in the grid.</param>
    public Tail(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    /// <summary>
    /// The total cost of the node (gCost + hCost).
    /// </summary>
    public int FCost
    {
        get { return gCost + hCost; }
    }
}


/// <summary>
/// A* pathfinding algorithm implementation.
/// </summary>
public class AStar : MonoBehaviour
{
    private TilesExecute m_tilesExecute;
    private GameObject[,] m_tailsGrid;

    private Tail[,] m_grid;

    private Vector2Int m_gameFieldSize;

    [HideInInspector]
    public bool m_isTailsGridCreated = false;

    private TailCenterCalculate m_tailCenterCalculate;

    private Vector2Int[] m_neighborsAround =
{
        new Vector2Int(-1, 0),
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(0, 1)
    };

    private List<Tail> m_path = new List<Tail>();

    #region Private Methods

    /// <summary>
    /// Initializes the AStar component.
    /// </summary>
    private void Start()
    {
        m_tilesExecute = GameObject.FindGameObjectWithTag("Tail").GetComponent<TilesExecute>();
        m_gameFieldSize = GameObject.FindGameObjectWithTag("GameField").GetComponent<GameField>().m_gameFieldSize;
        m_tailCenterCalculate = gameObject.AddComponent<TailCenterCalculate>();
    }

    /// <summary>
    /// Updates the component, checking for tail calculations and grid creation.
    /// </summary>
    private void Update()
    {
        if (m_tilesExecute != null && m_tilesExecute.m_tailsAreCalculate && !m_isTailsGridCreated)
        {
            m_isTailsGridCreated = true;
            m_tailsGrid = m_tilesExecute.m_tilesTwoDim;
            CreateTailGrid();
            m_tilesExecute.RemoveTailPrefabsFromGameField();
        }
    }

    /// <summary>
    /// Converts a world position to a corresponding grid tail.
    /// </summary>
    /// <param name="pos">The world position.</param>
    /// <returns>The corresponding grid tail.</returns>
    private Tail PositionAsGridTail(Vector3 pos)
    {
        Vector2 centeredPos = m_tailCenterCalculate.CenteringPosition(pos);
        //Debug.Log("centeredPos " + centeredPos);

        for (int y = 0; y < m_gameFieldSize.y; y++)
        {
            for (int x = 0; x < m_gameFieldSize.x; x++)
            {
                Tail tail = m_grid[y, x];
                if (tail.worldPosition.x == centeredPos.x && tail.worldPosition.z == centeredPos.y)
                {
                    return tail;
                }
            }
        }
        throw new InvalidOperationException("Tail not found for the given position!");
    }

    /// <summary>
    /// Calculates the Manhattan distance between two tails.
    /// </summary>
    /// <param name="tailA">First tail.</param>
    /// <param name="tailB">Second tail.</param>
    /// <returns>The Manhattan distance between the two tails.</returns>
    private int ManhattanDistance(Tail tailA, Tail tailB)
    {
        int dstX = Mathf.Abs(tailA.gridX - tailB.gridX);
        int dstY = Mathf.Abs(tailA.gridY - tailB.gridY);

        return dstX + dstY;
    }

    /// <summary>
    /// Gets the neighbors of a given tail in the grid.
    /// </summary>
    /// <param name="tail">The target tail.</param>
    /// <returns>The neighbors of the target tail.</returns>
    private List<Tail> GetNeighbors(Tail tail)
    {
        List<Tail> neighbors = new List<Tail>();

        foreach (var neigh in m_neighborsAround)
        {
            int checkX = tail.gridX + neigh.x;
            int checkY = tail.gridY + neigh.y;

            if (checkX >= 0 && checkX < m_gameFieldSize.y && checkY >= 0 && checkY < m_gameFieldSize.x)
            {
                neighbors.Add(m_grid[checkX, checkY]);
            }
        }

        return neighbors;
    }

    /// <summary>
    /// Finds the path from an enemy position to a player position using A*.
    /// </summary>
    /// <param name="enemyPosition">The position of the enemy.</param>
    /// <param name="playerPosition">The position of the player.</param>
    public void FindEnemyPathToPlayer(Vector3 enemyPosition, Vector3 playerPosition)
    {

        Tail playerPositionTail = PositionAsGridTail(playerPosition);
        Tail enemyPositionTail = PositionAsGridTail(enemyPosition);

        List<Tail> openSet = new List<Tail>();
        HashSet<Tail> closedSet = new HashSet<Tail>();
        openSet.Add(enemyPositionTail);

        while (openSet.Count > 0)
        {
            Tail currentTail = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentTail.FCost || (openSet[i].FCost == currentTail.FCost && openSet[i].hCost < currentTail.hCost))
                {
                    currentTail = openSet[i];
                }
            }

            openSet.Remove(currentTail);
            closedSet.Add(currentTail);

            if (currentTail == playerPositionTail)
            {
                RetracePath(enemyPositionTail, playerPositionTail);
                return;
            }

            foreach (Tail neighbor in GetNeighbors(currentTail))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int newCostToNeighbor = currentTail.gCost + ManhattanDistance(currentTail, neighbor);
                if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCostToNeighbor;
                    neighbor.hCost = ManhattanDistance(neighbor, playerPositionTail);
                    neighbor.parent = currentTail;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Retraces the path from the enemy tail to the player tail.
    /// </summary>
    /// <param name="enemyTail">The enemy tail.</param>
    /// <param name="playerTail">The player tail.</param>
    private void RetracePath(Tail enemyTail, Tail playerTail)
    {
        m_path = new List<Tail>();
        Tail currentTail = playerTail;

        while (currentTail != enemyTail)
        {
            m_path.Add(currentTail);
            currentTail = currentTail.parent;
        }

        m_path.Reverse();

        if (m_path.Count == 0)
            m_path.Add(currentTail);
    }

    /// <summary>
    /// Creates the grid of tails based on the tiles grid.
    /// </summary>
    private void CreateTailGrid()
    {
        m_grid = new Tail[m_gameFieldSize.y, m_gameFieldSize.x];

        for (int y = 0; y < m_gameFieldSize.y; y++)
        {
            for (int x = 0; x < m_gameFieldSize.x; x++)
            {
                if (m_tailsGrid[y, x].tag == "Barrier")
                {
                    m_grid[y, x] = new Tail(false, m_tailsGrid[y, x].transform.position, y, x);

                }
                else if (m_tailsGrid[y, x].tag == "Path")
                {
                    m_grid[y, x] = new Tail(true, m_tailsGrid[y, x].transform.position, y, x);

                }
            }
        }
    }

    #endregion


    #region Public Methods

    /// <summary>
    /// Gets the path calculated by A*.
    /// </summary>
    /// <param name="enemyPosition">The position of the enemy.</param>
    /// <param name="playerPosition">The position of the player.</param>
    /// <returns>The calculated path.</returns>
    public List<Tail> GetPath(Vector3 enemyPosition, Vector3 playerPosition)
    {
        FindEnemyPathToPlayer(enemyPosition, playerPosition);
        return m_path;
    }

    #endregion

}
