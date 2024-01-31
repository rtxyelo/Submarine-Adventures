using UnityEngine;


/// <summary>
/// Represents the game field and manages its initialization.
/// </summary>
public class GameField : MonoBehaviour
{
    [SerializeField]
    private Transform m_field;

    /// <summary>
    /// Gets the size of the game field.
    /// </summary>
    [HideInInspector]
    public Vector2Int m_gameFieldSize { get; private set; }

    /// <summary>
    /// Initializes the game field with the specified size.
    /// </summary>
    /// <param name="size">The size of the game field.</param>
    public void Initialize(Vector2Int size)
    {
        m_field.localScale = new Vector3(size.x, size.y, 1f);
        m_gameFieldSize = new Vector2Int(size.x, size.y);
    }
}
