using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public string volumeParameter = "MasterVolume";
    public AudioMixer AM;
    public Slider slider;
    private float _volumeValue;
    private float StartvolumeValue = 1f;
    private const float _multiplier = 20f;

    private void Awake()
    {
        slider.onValueChanged.AddListener(HandleSliderValueChanged);
    }

    private void HandleSliderValueChanged(float value)
    {
        _volumeValue = Mathf.Log10(value) * _multiplier;
        AM.SetFloat(volumeParameter, _volumeValue);
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey("volumeParameter"))
        {
            _volumeValue = PlayerPrefs.GetFloat(volumeParameter, Mathf.Log10(slider.value) * _multiplier);
            slider.value = Mathf.Pow(10f, _volumeValue / _multiplier);
        }
        else
        {
            PlayerPrefs.SetFloat(volumeParameter, StartvolumeValue);
        }
        
    }
    private void OnDisable()
    {
        PlayerPrefs.SetFloat(volumeParameter, _volumeValue);
    }
}
