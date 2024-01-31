using UnityEngine;


/// <summary>
/// Manages the overall behavior of the game.
/// </summary>
public class GameBehaviour : MonoBehaviour
{
    [SerializeField]
    private Vector2Int m_fieldsize;

    [SerializeField]
    private GameField _gameField;

    private void Awake()
    {
        _gameField.Initialize(m_fieldsize);
    }
}
