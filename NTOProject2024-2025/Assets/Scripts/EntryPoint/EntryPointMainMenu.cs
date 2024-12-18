using System;
using UnityEngine;

public class EntryPointMainMenu : MonoBehaviour
{
    [SerializeField] private VolumeSlider _volumeSliderMusic;
    [SerializeField] private VolumeSlider _volumeSliderEffects;

    private void Start()
    {
        InitializeData();
    }

    private void InitializeData()
    {
        _volumeSliderMusic.Initialization();
        _volumeSliderEffects.Initialization();
    }
}
