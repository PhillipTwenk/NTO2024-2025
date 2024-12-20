using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScreenResolutionControl : MonoBehaviour
{
    public TMP_Dropdown _dropdown;
    private Resolution[] _resolutions;
    private bool WhichScreenMode;
    private int WhichResolutionSaved;
    
    public void Initialization()
    {
        // Получаем разрешения экрана
        Resolution[] allResolutions = Screen.resolutions;

        // Убираем дубли по ширине и высоте
        _resolutions = allResolutions
            .GroupBy(res => new { res.width, res.height })
            .Select(group => group.First())
            .ToArray();

        // Преобразуем в строку для отображения в dropdown
        string[] strRes = new string[_resolutions.Length];
        for (int i = 0; i < _resolutions.Length; i++)
        {
            strRes[i] = _resolutions[i].width + "x" + _resolutions[i].height;
        }

        // Читаем настройки из PlayerPrefs
        WhichScreenMode = PlayerPrefs.HasKey("ScreenMode")
            ? Convert.ToBoolean(PlayerPrefs.GetInt("ScreenMode"))
            : true;

        WhichResolutionSaved = PlayerPrefs.HasKey("ResolutionMode")
            ? PlayerPrefs.GetInt("ResolutionMode")
            : _resolutions.Length - 1;

        if (!PlayerPrefs.HasKey("ResolutionMode"))
        {
            PlayerPrefs.SetInt("ResolutionMode", WhichResolutionSaved);
        }

        // Обновляем dropdown
        _dropdown.ClearOptions();
        _dropdown.AddOptions(strRes.ToList());
        _dropdown.value = WhichResolutionSaved;
        
        // Применяем разрешение
        Screen.SetResolution(_resolutions[WhichResolutionSaved].width, _resolutions[WhichResolutionSaved].height, WhichScreenMode);
    }

    public void SetResolution()
    {
        WhichScreenMode = PlayerPrefs.HasKey("ScreenMode")
            ? Convert.ToBoolean(PlayerPrefs.GetInt("ScreenMode"))
            : true;

        Screen.SetResolution(_resolutions[_dropdown.value].width, _resolutions[_dropdown.value].height, WhichScreenMode);
        PlayerPrefs.SetInt("ResolutionMode", _dropdown.value);
    }
}
