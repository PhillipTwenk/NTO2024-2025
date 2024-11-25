using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Скрипт, загружающий две первоначальные сцены, а также задающий первичные значения для всех PlayerPrefs и SOшек
/// Висит на объекте EntryPoint в сцене Bootstrap
/// </summary>
public class EntryPoint : MonoBehaviour
{
    [SerializeField] private GameEvent MoveToMainMenuSceneEvent;
    [SerializeField] private string PersistentManagerName;
    [SerializeField] private string MainMenuName;
    [SerializeField] private string BootstrapName;
    
    [SerializeField] private List<EntityID> playersList;
    [SerializeField] private List<Building> buildingsList;
    [SerializeField] private List<PlayerSaveData> playerSaveDatas;
    
    [SerializeField] private bool IsInEditor;
    
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
        
        //TestInEditor
        if (IsInEditor)
        {
            foreach (var player in playersList)
            {
                player.DefaultRevert();
            }

            // foreach (var building in buildingsList)
            // {
            //     building.DefaultRevert();
            // }

            foreach (var psd in playerSaveDatas)
            {
                psd.RevertBuildingsData();
            }
        }

        InitializeData();
        
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
