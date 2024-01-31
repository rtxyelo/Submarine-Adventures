using UnityEngine;


/// <summary>
/// Manages the life count of the player using PlayerPrefs.
/// </summary>
public class LifeCountBehaviour : MonoBehaviour
{
    private string m_healthKey = "HealthKey";

    private int m_health = 3;

    private void Awake()
    {
        // If the health key doesn't exist, set it to the default health value.
        if (!PlayerPrefs.HasKey(m_healthKey))
            PlayerPrefs.SetInt(m_healthKey, m_health);
    }

    private void Start()
    {
        // Set the player's health to the default value.
        PlayerPrefs.SetInt(m_healthKey, m_health);
    }
}
