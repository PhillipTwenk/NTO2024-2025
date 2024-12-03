using System;
using UnityEngine;

public class PauseUIManager : MonoBehaviour
{
    [SerializeField] private GameEvent PauseOnEvent;
    [SerializeField] private GameEvent PauseOffEvent;
    [SerializeField] private GameEvent ClickSettingsPauseEvent;
    [SerializeField] private GameEvent ArriveToPauseMenuEvent;

    [SerializeField] private GameObject PausePanel;

    public void PauseOn() => PauseOnEvent.TriggerEvent();
    public void PauseOff() => PauseOffEvent.TriggerEvent();
    public void ClickSettingsPause() => ClickSettingsPauseEvent.TriggerEvent();
    public void ArriveToPauseMenu() => ArriveToPauseMenuEvent.TriggerEvent();

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            PauseResume();
        }
    }
    
    public void PauseResume()
    {
        if (!PausePanel.activeSelf)
        {
            Debug.Log("Пауза");
            Time.timeScale = 0f;
            PauseOn();
        }
        else
        {
            Debug.Log("Продолжаем");
            Time.timeScale = 1f;
            PauseOff();
        }
    }
    
    public void WindowScreen()
    {
        Screen.SetResolution(Screen.width, Screen.height, false);
        PlayerPrefs.SetString("ScreenMode", "Оконный");
        Debug.Log("Оконный");
    }
    public void FullScreen()
    {
        Screen.SetResolution(Screen.width, Screen.height, true, 60);
        PlayerPrefs.SetString("ScreenMode", "Полноэкранный");
        Debug.Log("Полноэкранный");
    }
    
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
