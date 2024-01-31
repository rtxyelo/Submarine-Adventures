using System.Collections;
using UnityEngine;


/// <summary>
/// Controls the behavior of the camera in relation to the player, including shaking effects.
/// </summary>
public class CameraBehaviour : MonoBehaviour
{
    [SerializeField]
    private Transform m_playerTransform;
    private Vector3 m_offset = new Vector3(0f, 4.0f, 2.0f);
    private static bool m_isShaking = false;

    private void Start()
    {
        if (m_playerTransform == null)
        {
            Debug.LogWarning("Player Transform is not assigned!");
        }
    }

    private void Update()
    {
        transform.position = m_playerTransform.position + m_offset;
    }

    /// <summary>
    /// Shakes the camera for a specified duration and intensity.
    /// </summary>
    /// <param name="duration">The duration of the shaking effect.</param>
    /// <param name="intensity">The intensity of the shaking effect.</param>
    /// <returns>An IEnumerator used for coroutine behavior.</returns>
    public IEnumerator ShakeCamera(float duration, float intensity)
    {
        if (m_isShaking) yield break;

        m_isShaking = true;

        Vector3 originalPosition = m_playerTransform.position + m_offset;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * intensity;
            transform.position = originalPosition + shakeOffset;
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.position = originalPosition;
        m_isShaking = false;
    }
}
