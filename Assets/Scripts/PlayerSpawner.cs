using UnityEngine;


/// <summary>
/// Spawns the player character on a random path tile for MLAgent learning process. (NO ACTUAL!)
/// </summary>
public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    private TilesExecute m_tilesExecute;

    private GameObject[] m_pathTiles;

    private int m_tileRandomNum;
    private bool m_isRecalculate = false;
    private Transform pathTileTransform;
    private static int PATH_TILES_COUNT = 149;


    /// <summary>
    /// Respawns the player on a random path tile.
    /// </summary>
    public void RespawnPlayer()
    {
        if (!m_isRecalculate)
        {
            m_isRecalculate = true;
            m_tilesExecute.RecalculateTiles();
        }

        m_tileRandomNum = Random.Range(0, PATH_TILES_COUNT);
        m_pathTiles = m_tilesExecute.m_pathTails;
                                    
        if (m_tileRandomNum < m_pathTiles.Length)
        {
            pathTileTransform = m_pathTiles[m_tileRandomNum].transform;
            transform.position = pathTileTransform.position;
        }
        else
        {
            Debug.LogError("Invalid random tile index!");
        }
    }
}
