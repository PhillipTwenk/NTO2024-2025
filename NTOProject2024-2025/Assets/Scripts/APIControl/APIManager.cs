using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Класс для получения нужных ссылок в зависимости от задачи
/// </summary>
public class Requests
{
    private static string UUID = "989f4271-59bd-4b47-b2a1-e2d838207f6e";
    public static string CreatePlayerURL = $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/";
    public static string GetPlayersURL = $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/";

    public static string GetPlayerURL(string playerName) =>  $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/{playerName}";

    public static string PutPlayerURL(string playerName) => $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/{playerName}";

    public static string DeletePlayerURL(string playerName) => $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/{playerName}";

    public static string CreateLogURL = $"https://2025.nti-gamedev.ru/api/games/{UUID}/logs/";

    public static string GetPlayerLogsURL(string playerName) => $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/{playerName}/";

    public static string CreateShopURL(string playerName) => $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/{playerName}/shops/";

    public static string GetShopsURL(string playerName) => $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/{playerName}/shops/";

    public static string GetShopURL(string playerName, string shopName) => $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/{playerName}/shops/{shopName}/";

    public static string PutShopURL(string playerName, string shopName) => $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/{playerName}/shops/{shopName}/";

    public static string DeleteShopURL(string playerName, string shopName) => $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/{playerName}/shops/{shopName}/";

    public static string CreateShopLogURL = $"https://2025.nti-gamedev.ru/api/games/{UUID}/logs/";

    public static string GetShopLogURL(string playerName, string shopName) => $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/{playerName}/shops/{shopName}/logs/";

    public static string GetLogGameURL = $"https://2025.nti-gamedev.ru/api/games/{UUID}/logs/";
}
public class APIManager : MonoBehaviour
{
    public static APIManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Создание персонажа
    /// </summary>
    /// <param name="playerName"> Имя персонажа</param>
    /// <param name="playerIron"> Количество металла </param>
    /// <param name="playerEnergy"> Количество энергии </param>
    /// <param name="playerFood"> Количество пищи </param>
    /// <param name="playerCrioCrystal"> Количество Криокристаллов </param>
    public void CreatePlayer(string playerName, int playerIron, int playerEnergy, int playerFood, int playerCrioCrystal)
    {
        PlayerData playerData = new PlayerData()
        {
            name = playerName,
            resources = new PlayerResources()
            {
                Iron = playerIron,
                Energy = playerEnergy,
                Food = playerFood,
                CrioCrystal = playerCrioCrystal
            }
        };
        
        string json = JsonUtility.ToJson(playerData, true);
        
        HTTPRequests.Instance.Post(Requests.CreatePlayerURL, json, 
            onSuccess: responce =>
            {
               Debug.Log("Персонаж успешно создан");
            },
            onError: responce =>
            {
                Debug.Log("Возникла ошибка при создании персонажа");
            });
    }
    
    /// <summary>
    /// Возвращает Dictionary cо всеми игроками
    /// </summary>
    /// <returns></returns>
    public async Task<Dictionary<string, PlayerData>> GetPlayersList()
    {
        // Создаем TaskCompletionSource для обработки асинхронного ответа
        var taskCompletionSource = new TaskCompletionSource<Dictionary<string, PlayerData>>();

        string URL = Requests.GetPlayersURL;
        
        HTTPRequests.Instance.Get(URL,
            onSuccess: response =>
            {
                try
                {
                    // Парсим ответ
                    List<PlayerData> players = JsonUtility.FromJson<PlayerDataList>($"{{\"players\":{response}}}").players;

                    // Проверяем, есть ли игроки
                    var playerDict = new Dictionary<string, PlayerData>();
                    foreach (var player in players)
                    {
                        if (!playerDict.ContainsKey(player.name))
                        {
                            playerDict[player.name] = player;
                        }
                    }

                    // Завершаем Task успешным результатом
                    taskCompletionSource.SetResult(playerDict);
                }
                catch (Exception ex)
                {
                    // Завершаем Task с ошибкой при возникновении исключения
                    Debug.LogError($"Ошибка при обработке данных: {ex.Message}");
                    taskCompletionSource.SetException(ex);
                }
            },
            onError: error =>
            {
                // Завершаем Task с ошибкой при проблемах с запросом
                Debug.LogError($"Ошибка запроса: {error}");
                taskCompletionSource.SetException(new Exception(error));
            });

        // Ждем завершения Task и возвращаем результат
        return await taskCompletionSource.Task;
    }

    
    /// <summary>
    /// Получение ресурсов игрока 
    /// </summary>
    /// <param name="playerName"></param>
    /// <returns></returns>
    public async Task<PlayerResources> GetPlayerResources(string playerName)   
    {
        string URL = Requests.GetPlayerURL(playerName);

        // Создаем TaskCompletionSource для ожидания результата запроса
        var taskCompletionSource = new TaskCompletionSource<PlayerResources>();

        HTTPRequests.Instance.Get(URL, 
            onSuccess: response =>
            {
                Debug.Log("Данные о ресурсах персонажа успешно получены");
                PlayerData playerData = JsonUtility.FromJson<PlayerData>(response);
                taskCompletionSource.SetResult(playerData.resources); // Устанавливаем результат
            },
            onError: error =>
            {
                Debug.LogError("Возникла ошибка при получении данных о персонаже: " + error);
                taskCompletionSource.SetException(new Exception(error)); // Устанавливаем исключение
            });

        // Ждем завершения Task и возвращаем результат
        return await taskCompletionSource.Task;
    }


    /// <summary>
    /// Загрузка новых ресурсов определенному игроку
    /// </summary>
    /// <param name="playerName"> Имя игрока </param>
    /// <param name="playerIron"> Металл </param>
    /// <param name="playerEnergy"> Энергомед </param>
    /// <param name="playerFood"> Еда </param>
    /// <param name="playerCrioCrystal"> Криосталы </param>
    public void PutPlayerResources(string playerName, int playerIron, int playerEnergy, int playerFood, int playerCrioCrystal)
    {
        PlayerResources resources = new PlayerResources()
        {
            Iron = playerIron,
            Energy = playerEnergy,
            Food = playerFood,
            CrioCrystal = playerCrioCrystal
        };
        
        string json = JsonUtility.ToJson(resources, true);

        string URL = Requests.PutPlayerURL(playerName);
        
        
        HTTPRequests.Instance.Put(URL, json,
            onSuccess: responce =>
            {
                Debug.Log("Ресурсы персонажа успешно обновлены");
            },
            onError: responce =>
            {
                Debug.Log("Возникла ошибка при обновлении ресурсов персонажа");
            });
    }

    /// <summary>
    /// Удаление игрока
    /// </summary>
    /// <param name="playerName"></param>
    public void DeletePlayer(string playerName)
    {
        string URL = Requests.DeletePlayerURL(playerName);
        HTTPRequests.Instance.Delete(URL,
            onSuccess: responce =>
            {
                Debug.Log("Персонаж успешно удален");
            },
            onError: responce =>
            {
                Debug.Log("Возникла ошибка при удалении персонажа");
            });
    }

    public void CreatePlayerLog(string comment, string playerName, string changedIron, string changedEnergy, string changedFood, string changedCrioCrystal)
    {
        
    }

    public PlayerLog GetPlayerLogs()
    {
        PlayerLog playerLog = new PlayerLog();
        return playerLog;
    }

    public void CreateShop(string shopName, int ApiaryShop, int HoneyGunShop, int MobileBaseShop, int StorageShop, int ResidentialModuleShop, int BreadwinnerShop)
    {
        
    }

    public List<ShopData> GetShopsList()
    {
        List<ShopData> shopList = new List<ShopData>();
        return shopList;
    }

    public ShopResources GetShopResources(string shopName)
    {
        ShopResources resources = new ShopResources();
        return resources;
    }

    public void PutShopResources(string shopName, int ApiaryShop, int HoneyGunShop, int MobileBaseShop, int StorageShop, int ResidentialModuleShop, int BreadwinnerShop)
    {
        
    }

    public void DeleteShop(string shopName)
    {
        
    }
    
    public void CreateShopLog(string comment, string playerName, string shopName, string ChangedApiaryShop, string ChangedHoneyGunShop, string ChangedMobileBaseShop, string ChangedStorageShop, string ChangedResidentialModuleShop, string ChangedBreadwinnerShop)
    {
        
    }

    public ShopLog GetShopLog(string shopName)
    {
        ShopLog shopLog = new ShopLog();
        return shopLog;
    } 

}
