using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Handles the movement type selection in the game.
/// </summary>
public class MovementBehaviour : MonoBehaviour
{
    [SerializeField]
    private Sprite m_uncheckedSprite;

    [SerializeField]
    private Sprite m_checkedSprite;

    [SerializeField]
    private Button m_arcadeModeBtn;

    [SerializeField]
    private Button m_simulatorModeBtn;

    private string m_movementKey = "MovementKey";

    private int m_movementType = 0;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(m_movementKey))
        {
            PlayerPrefs.SetInt(m_movementKey, m_movementType);
            m_arcadeModeBtn.image.sprite = m_checkedSprite;
            m_simulatorModeBtn.image.sprite = m_uncheckedSprite;
        }
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt(m_movementKey, 0) == 1)
        {
            m_arcadeModeBtn.image.sprite = m_uncheckedSprite;
            m_simulatorModeBtn.image.sprite = m_checkedSprite;
        }
        else
        {
            m_arcadeModeBtn.image.sprite = m_checkedSprite;
            m_simulatorModeBtn.image.sprite = m_uncheckedSprite;
        }
    }

    /// <summary>
    /// Changes the movement type based on the specified value.
    /// </summary>
    /// <param name="moveType">The new movement type (0 or 1).</param>
    public void ChangeMovementType(int moveType)
    {
        if (moveType == 0 || moveType == 1)
        {
            PlayerPrefs.SetInt(m_movementKey, moveType);
        }
        else
        {
            Debug.LogWarning("Unknown type of movement! Default value is used!");
            PlayerPrefs.SetInt(m_movementKey, m_movementType);
        }
    }
}
