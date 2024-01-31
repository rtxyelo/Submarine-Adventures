using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Manages the volume control for background music in the game. (FROM PREVIOUS PROJECT)
/// </summary>
public class SoundBehaviour : MonoBehaviour
{
    [SerializeField] private AudioSource _music;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private Button _musicButton;
    [SerializeField] private Sprite _iconOn, _iconOff;

    private string _musicVolumeKey = "MusicVolumeKey";

    /// <summary>
    /// Initializes the volume settings based on PlayerPrefs when the script is started.
    /// </summary>
    private void Start()
    {
        if (!PlayerPrefs.HasKey(_musicVolumeKey))
        {
            PlayerPrefs.SetFloat(_musicVolumeKey, 0.2f);
            if (_musicButton != null)
            {
                _musicButton.image.sprite = _iconOn;
                _volumeSlider.value = PlayerPrefs.GetFloat(_musicVolumeKey);

            }
        }
        else if (PlayerPrefs.GetFloat(_musicVolumeKey) > 0)
        {
            if (_musicButton != null)
            {
                _musicButton.image.sprite = _iconOn;
                _volumeSlider.value = PlayerPrefs.GetFloat(_musicVolumeKey);
            }
        }
        else
        {
            if (_musicButton != null)
            {
                _musicButton.image.sprite = _iconOff;
                _volumeSlider.value = PlayerPrefs.GetFloat(_musicVolumeKey);
            }
        }
        _music.volume = PlayerPrefs.GetFloat(_musicVolumeKey);
    }

    /// <summary>
    /// Updates the volume settings based on the slider value.
    /// </summary>
    public void OnSliderChange()
    {
        PlayerPrefs.SetFloat(_musicVolumeKey, _volumeSlider.value);
        _music.volume = PlayerPrefs.GetFloat(_musicVolumeKey);
        if (PlayerPrefs.GetFloat(_musicVolumeKey) > 0)
        {
            if (_musicButton != null)
            {
                _musicButton.image.sprite = _iconOn;
                _volumeSlider.value = PlayerPrefs.GetFloat(_musicVolumeKey);
            }
        }
        else
        {
            if (_musicButton != null)
            {
                _musicButton.image.sprite = _iconOff;
                _volumeSlider.value = PlayerPrefs.GetFloat(_musicVolumeKey);
            }
        }
    }

    /// <summary>
    /// Toggles the volume on/off and updates the UI accordingly.
    /// </summary>
    public void SwitchVolume()
    {
        if (PlayerPrefs.GetFloat(_musicVolumeKey) > 0)
        {
            _musicButton.image.sprite = _iconOff;
            PlayerPrefs.SetFloat(_musicVolumeKey, 0);
            _music.volume = PlayerPrefs.GetFloat(_musicVolumeKey);
            _volumeSlider.value = PlayerPrefs.GetFloat(_musicVolumeKey);
        }
        else
        {
            _musicButton.image.sprite = _iconOn;
            PlayerPrefs.SetFloat(_musicVolumeKey, 0.2f);
            _music.volume = PlayerPrefs.GetFloat(_musicVolumeKey);
            _volumeSlider.value = PlayerPrefs.GetFloat(_musicVolumeKey);
        }
    }
}
