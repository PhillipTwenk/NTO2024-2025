using System;
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

    // [SerializeField] private SettingsSaveConfiguration settings;
    // private float VolumeParamSettings;

    public void Initialization()
    {
        slider.onValueChanged.AddListener(HandleSliderValueChanged);
        if (PlayerPrefs.HasKey(volumeParameter))
        {
            _volumeValue = PlayerPrefs.GetFloat(volumeParameter, Mathf.Log10(slider.value) * _multiplier);
            slider.value = Mathf.Pow(10f, _volumeValue / _multiplier);
        }
        else
        {
            PlayerPrefs.SetFloat(volumeParameter, StartvolumeValue);
        }
    }

    private void HandleSliderValueChanged(float value)
    {
        if (value <= 0.0001f) // Проверяем на очень малое значение, чтобы избежать проблем с точностью.
        {
            _volumeValue = -80f; // Устанавливаем минимальное значение громкости.
        }
        else
        {
            _volumeValue = Mathf.Log10(value) * _multiplier;
        }

        AM.SetFloat(volumeParameter, _volumeValue);
    }

    private void Start()
    {
        Initialization();
    }
    private void OnDisable()
    {
        PlayerPrefs.SetFloat(volumeParameter, _volumeValue);
    }
}