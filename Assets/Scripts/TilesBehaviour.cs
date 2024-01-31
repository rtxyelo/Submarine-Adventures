using UnityEngine;

/// <summary>
/// Handles collision events related to tiles.
/// </summary>
public class TilesBehaviour : MonoBehaviour
{
    private TilesExecute m_tilesExecute;

    /// <summary>
    /// Called when a collision is detected and the colliding object has the "Rock" tag.
    /// Resets the path tails using the TilesExecute component.
    /// </summary>
    /// <param name="collision">The collision information.</param>
    private void OnCollisionStay(Collision collision)
    {
        if (collision != null && collision.transform.gameObject.CompareTag("Rock"))
        {
            m_tilesExecute = GetComponentInParent<TilesExecute>();
            m_tilesExecute.ResetPathTails(gameObject);
        }
    }
}
