using System.Collections;
using UnityEngine;


/// <summary>
/// Represents information about a mine, including its position.
/// </summary>
public class MineInfo
{
    public Vector3 MinePosition { get; }

    public MineInfo(Vector3 minePos)
    {
        MinePosition = minePos;
    }
}


/// <summary>
/// Handles the behavior of a mine in the game.
/// </summary>
public class MineBehaviour : MonoBehaviour
{
    [SerializeField]
    private float m_explodeTime = 3f;

    [SerializeField]
    private GameObject m_explosionPrefab;

    [SerializeField]
    private LayerMask m_layerMask;

    [SerializeField]
    private MeshRenderer[] m_mineMeshRenderer;

    [SerializeField]
    private AudioClip m_explosionSound;
    private string _musicVolumeKey = "MusicVolumeKey";

    private PlayerBehaviour m_playerBehaviour;
    private CameraBehaviour m_cameraBehaviour;


    #region Private Methods

    private void Start()
    {
        m_cameraBehaviour = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraBehaviour>();
        m_playerBehaviour = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        Invoke(nameof(Explode), m_explodeTime);
    }

    /// <summary>
    /// Initiates the explosion sequence of the mine.
    /// </summary>
    private void Explode()
    {
        if (m_cameraBehaviour != null)
        {
            StartCoroutine(m_cameraBehaviour.ShakeCamera(0.2f, 0.2f));
        }

        foreach (var item in m_mineMeshRenderer)
        {
            item.enabled = false;
        }

        if (m_playerBehaviour != null)
            m_playerBehaviour.MineIsExploded(new MineInfo(transform.position));

        Instantiate(m_explosionPrefab, transform.position, Quaternion.identity);

        Invoke(nameof(StartCoroutine), 0.07f);

        if (m_explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(m_explosionSound, transform.position, PlayerPrefs.GetFloat(_musicVolumeKey, 0.2f));
        }

        Destroy(gameObject, 1.0f);
    }

    /// <summary>
    /// Starts coroutine with delay.
    /// </summary>
    private void StartCoroutine()
    {
        StartCoroutine(CreateExplodeWave(Vector3.forward));
        StartCoroutine(CreateExplodeWave(Vector3.back));
        StartCoroutine(CreateExplodeWave(Vector3.left));
        StartCoroutine(CreateExplodeWave(Vector3.right));
    }

    /// <summary>
    /// Creates an exploding wave in the specified direction.
    /// </summary>
    private IEnumerator CreateExplodeWave(Vector3 dir)
    {
        for (int i = 1; i < 4; i++)
        {
            Physics.Raycast(transform.position + new Vector3(0f, -0.5f, 0f), dir, out RaycastHit hit, i, m_layerMask);

            if (!hit.collider)
            {
                Instantiate(m_explosionPrefab, transform.position + (i * dir), Quaternion.identity);
            }
            else
                break;

            yield return new WaitForSeconds(0.07f);
        }
    }

    #endregion

}