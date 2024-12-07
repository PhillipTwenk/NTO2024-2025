using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField] private GameEvent EndMoveToSceneLocationEvent;
    [SerializeField] private GameEvent StartTutorial;
    [SerializeField] private string LocationSceneName;
    [SerializeField] private string MainMenuSceneName;
    [SerializeField] private string UISceneName;
    private bool IsNewPlayer;


    public void NewPlayer() => IsNewPlayer = true;
    
    /// <summary>
    /// Метод перехода на основную сцену
    /// </summary>
    public void MoveToSceneLocation()
    {
        StartCoroutine(MoveToSceneLocationCoroutine());
    }
    
    /// <summary>
    /// Корутина, реализующая загрузку сцен и запускающая ивент окончания перехода на первую сцену
    /// </summary>
    private IEnumerator MoveToSceneLocationCoroutine()
    {
        EntityID ActivePlayer = UIManagerMainMenu.WhichPlayerCreate;
        // //Панель загрузки
        // LoadingCanvas.SetActive(true);
        
        Debug.Log("----- Переход на игровую сцену -----");
        
        //Загрузка уровня
        AsyncOperation LoadingSceneLocation = 
            SceneManager.LoadSceneAsync(LocationSceneName, LoadSceneMode.Additive);
        yield return new WaitUntil(()=>LoadingSceneLocation.isDone);

        Debug.Log(1111111111111111111);
        UIManagerLocation.WhichPlayerCreate = ActivePlayer;

        //Установка уровня как основной сцены
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(LocationSceneName));
        
        //Проверка на активность нужных сцен, и их выгрузка
        bool isSceneMainMenuActive = SceneManager.GetSceneByName(MainMenuSceneName).isLoaded;
        if (isSceneMainMenuActive)
        {
            AsyncOperation UnloadingMainMenu = SceneManager.UnloadSceneAsync(MainMenuSceneName);
            //yield return new WaitUntil(()=>UnloadingMainMenu.isDone);
        }
        
        
        //Проверка, если UI цена не активна, загружаем её
        bool isSceneUIActive = SceneManager.GetSceneByName(UISceneName).isLoaded;
        if (!isSceneUIActive)
        {
            AsyncOperation UILoadingScene = SceneManager.LoadSceneAsync(UISceneName, LoadSceneMode.Additive);
            yield return new WaitUntil(()=>UILoadingScene.isDone);
        }
        
        //LoadingCanvas.SetActive(false);
        EndMoveToSceneLocationEvent.TriggerEvent();
        
        if (IsNewPlayer)
        {
            StartTutorial.TriggerEvent();
        }
    }
}
