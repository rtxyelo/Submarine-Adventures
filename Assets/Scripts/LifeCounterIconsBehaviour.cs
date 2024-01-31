using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Manages the visual representation of the player's life count using life icons.
/// </summary>
public class LifeCounterBehaviour : MonoBehaviour
{

    [SerializeField]
    private GameObject m_lifeIconPrefab;

    private string m_healthKey = "HealthKey";

    private int m_health;

    private List<GameObject> m_lifeIconsList = new List<GameObject>();

    private void Start()
    {
        // Retrieve the player's health from PlayerPrefs.
        if (PlayerPrefs.HasKey(m_healthKey))
        {
            m_health = PlayerPrefs.GetInt(m_healthKey, 3);
        }

        // Instantiate life icons based on the player's health.
        for (int i = 0; i < m_health;  ++i)
        {
            m_lifeIconsList.Add(Instantiate(m_lifeIconPrefab, transform));
        }
    }

    /// <summary>
    /// Adds a life icon to the visual representation.
    /// </summary>
    public void AddLifeIcon()
    {
        m_lifeIconsList.Add(Instantiate(m_lifeIconPrefab, transform));
    }
}
