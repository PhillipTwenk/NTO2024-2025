using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Скрипт, загружающий две первоначальные сцены, а также задающий первичные значения для всех PlayerPrefs и SOшек
/// Висит на объекте EntryPoint в сцене Bootstrap
/// </summary>
public class EntryPoint : MonoBehaviour
{
    
    [SerializeField] private bool IsInEditor;


    [SerializeField] private GameEvent MoveToMainMenuSceneEvent;
    [SerializeField] private string PersistentManagerName;
    [SerializeField] private string MainMenuName;
    [SerializeField] private string BootstrapName;

    [SerializeField] private EntityID player1;
    [SerializeField] private EntityID player2;
    [SerializeField] private EntityID player3;

    private void Awake()
    {
        //TestInEditor
        if (IsInEditor)
        {
            player1.DefaultRevert();
            player2.DefaultRevert();
            player3.DefaultRevert();
        }
    }

    /// <summary>
    ///Запускает инициализацию данных и загружает две изначальные сцены, после чего выгружает Bootstrap 
    /// </summary>
    private IEnumerator Start()
    {
        InitializeData();
        
        AsyncOperation LoadingScenePersistentManagers =
            SceneManager.LoadSceneAsync(PersistentManagerName, LoadSceneMode.Additive);
        AsyncOperation LoadingSceneMainMenu = 
            SceneManager.LoadSceneAsync(MainMenuName, LoadSceneMode.Additive);
            
            
        //Пока не завершится выгрузка сцены точки вхождения
        yield return new WaitUntil(()=>LoadingSceneMainMenu.isDone && LoadingScenePersistentManagers.isDone);
        Debug.Log("PersistentManagers и MainMenu сцены были загружены");
        
        //Установка сцены главного меню основной
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(MainMenuName));
        
        MoveToMainMenuSceneEvent.TriggerEvent();
        
        //Выгрузка сцены - точки входа
        SceneManager.UnloadSceneAsync(BootstrapName);
    }

    private void InitializeData()
    {
        
    }
}
