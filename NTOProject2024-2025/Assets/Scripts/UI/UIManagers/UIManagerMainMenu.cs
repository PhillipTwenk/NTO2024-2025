using UnityEngine;

public class UIManagerMainMenu : MonoBehaviour
{
    [SerializeField] private GameEvent StartGameButtonClickEvent;
    [SerializeField] private GameEvent SettingsButtonClickEvent;
    [SerializeField] private GameEvent ReturnToMainMenuEvent;
    [SerializeField] private GameEvent ReturnToPlayerChoicePanelEvent;
    [SerializeField] private GameEvent ClickCreateNewPlayerButtonEvent;
    [SerializeField] private GameEvent StartGameInChoiceCharacterPanel;
    //[SerializeField] private EntityID player1;
    //[SerializeField] private EntityID player2;
    //[SerializeField] private EntityID player3;
    
        
        
        
    /// <summary>
    /// Запускает игру по нажатию на кнопку старта
    /// </summary>
    public void StartGameClickButton()
    {
        Debug.Log("Нажата кнопка Старта игры");
        StartGameButtonClickEvent.TriggerEvent();
    }
    
    /// <summary>
    /// Открывает настройки
    /// </summary>
    public void SettingsClickButton()
    {
        Debug.Log("Нажата кнопка настроек");
        SettingsButtonClickEvent.TriggerEvent();
    }
    
    
    /// <summary>
    /// При загрузке главного меню, берем сохраненное значение ключа и устанавливаем режим игры
    /// Метод загружатеся в GameEventListener и исполняется при активации ивента MoveToMainMenuSceneEvent 
    /// </summary>
    public void StartGameScreenMode()
    {
        if (PlayerPrefs.HasKey("ScreenMode"))
        {
            if (PlayerPrefs.GetString("ScreenMode") == "Полноэкранный")
            {
                Debug.Log("Установлен полноэкранный режим");
                Screen.SetResolution(Screen.width, Screen.height, false);
            }
            
            if (PlayerPrefs.GetString("ScreenMode") == "Оконный")
            {
                Debug.Log("Установлен оконный режим");
                Screen.SetResolution(Screen.width, Screen.height, true, 60);
            }
        }
        else
        {
            Debug.Log("Установлен полноэкранный режим");
            Screen.SetResolution(Screen.width, Screen.height, false);
        }
    }
    
    /// <summary>
    /// Установка полноэкранного режима
    /// </summary>
    public void WindowScreen()
    {
        Screen.SetResolution(Screen.width, Screen.height, true, 60);
        PlayerPrefs.SetString("ScreenMode", "Оконный");
        Debug.Log("Оконный");
    }
    
    
    /// <summary>
    /// Установка оконного режима
    /// </summary>
    public void FullScreen()
    {
        Screen.SetResolution(Screen.width, Screen.height, false);
        PlayerPrefs.SetString("ScreenMode", "Полноэкранный");
        Debug.Log("Полноэкранный");
    }
    
    
    /// <summary>
    /// Выход из игры
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
    
    /// <summary>
    /// Вернуться в основное главное меню
    /// </summary>
    public void ReturnToMainMenu()
    {
        Debug.Log("Возврат в Главное меню");
        ReturnToMainMenuEvent.TriggerEvent(); 
    }
    
    /// <summary>
    /// Вернуться в меню выбора персонажа
    /// </summary>
    public void ReturnToPlayerChoicePanel()
    {
        Debug.Log("Возврат в меню выбора персонажа");
        ReturnToPlayerChoicePanelEvent.TriggerEvent(); 
    }
    
    /// <summary>
    /// Нажатие на кнопку создания 
    /// </summary>
    public void ChoiceNewPlayer(EntityID player)
    {
        if (player.Name == "None")
        {
            Debug.Log("Создание нового персонажа");
            ClickCreateNewPlayerButtonEvent.TriggerEvent(); 
        }
        else
        {
            Debug.Log("Вход в игру с существующим персонажем");
            StartGameInChoiceCharacterPanel.TriggerEvent();
        }
        
    }
    
//     /// <summary>
//     /// Начать игру после создания персонажа
//     /// </summary>
//     public void StartGameAfterCreateChoice()
//     {
//         Debug.Log("Вход в игру с существующим персонажем");
//         StartGameInChoiceCharacterPanel.TriggerEvent(); 
//     }
}