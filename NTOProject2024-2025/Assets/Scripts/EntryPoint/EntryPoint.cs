using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Скрипт, загружающий две первоначальные сцены, а также задающий первичные значения для всех PlayerPrefs и SOшек
/// Висит на объекте EntryPoint в сцене Bootstrap
/// </summary>
public class EntryPoint : MonoBehaviour
{
    [SerializeField] private GameEvent MoveToMainMenuSceneEvent;
    
    /// <summary>
    ///Запускает инициализацию данных и загружает две изначальные сцены, после чего выгружает Bootstrap 
    /// </summary>
    private IEnumerator Start()
    {
        InitializeData();
        
        AsyncOperation LoadingScenePersistentManagers =
            SceneManager.LoadSceneAsync("PersistentManagers", LoadSceneMode.Additive);
        AsyncOperation LoadingSceneMainMenu = 
            SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);
            
            
        //Пока не завершится выгрузка сцены точки вхождения
        yield return new WaitUntil(()=>LoadingSceneMainMenu.isDone && LoadingScenePersistentManagers.isDone);
        Debug.Log("PersistentManagers и MainMenu сцены были загружены");
        
        //Установка сцены главного меню основной
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainMenu"));
        
        MoveToMainMenuSceneEvent.TriggerEvent();
        
        //Выгрузка сцены - точки входа
        SceneManager.UnloadSceneAsync("Bootstrap");
    }

    private void InitializeData()
    {
        
    }
}
