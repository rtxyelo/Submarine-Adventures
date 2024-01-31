using UnityEngine;

/// <summary>
/// Handles the audio behavior for the prize object.
/// </summary>
public class PrizeSoundBehaviour : MonoBehaviour
{
    [SerializeField]
    private AudioClip m_prizeSoundClip;

    private AudioSource m_prizeAudioSource;
    private string _musicVolumeKey = "MusicVolumeKey";

    private void Start()
    {
        InitializeAudioSource();
        PlayPrizeSound();
    }

    /// <summary>
    /// Initializes the audio source component.
    /// </summary>
    private void InitializeAudioSource()
    {
        m_prizeAudioSource = gameObject.GetComponent<AudioSource>();
        m_prizeAudioSource.clip = m_prizeSoundClip;
        m_prizeAudioSource.volume = PlayerPrefs.HasKey(_musicVolumeKey) ? PlayerPrefs.GetFloat(_musicVolumeKey, 0.2f) : 0.2f;
        m_prizeAudioSource.loop = true;
    }

    /// <summary>
    /// Plays the prize sound.
    /// </summary>
    private void PlayPrizeSound()
    {
        m_prizeAudioSource.Play();
    }
}
