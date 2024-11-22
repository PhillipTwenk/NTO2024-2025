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

    public static string GetPlayerURL(string playerName) =>  $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/{playerName}/";

    public static string PutPlayerURL(string playerName) => $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/{playerName}/";

    public static string DeletePlayerURL(string playerName) => $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/{playerName}/";

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
    public async Task CreatePlayer(string playerName, int playerIron, int playerEnergy, int playerFood, int playerCrioCrystal)
    {
        // Создаем объект PlayerData
        PlayerData playerData = new PlayerData()
        {
            name = playerName,
            resources = new PlayerResources()
            {
                Iron = playerIron,
                Energy = playerEnergy,
                Food = playerFood,
                CryoCrystal = playerCrioCrystal
            }
        };

        // Преобразуем в JSON
        string json = JsonUtility.ToJson(playerData, true);

        // Создаем TaskCompletionSource для ожидания ответа
        var taskCompletionSource = new TaskCompletionSource<bool>();

        // Выполняем POST-запрос
        HTTPRequests.Instance.Post(Requests.CreatePlayerURL, json, 
            onSuccess: response =>
            {
                Debug.Log("Персонаж успешно создан");
                taskCompletionSource.SetResult(true); // Завершаем Task успешным результатом
            },
            onError: error =>
            {
                Debug.LogError($"Ошибка при создании персонажа: {error}");
                taskCompletionSource.SetException(new Exception(error)); // Завершаем Task с ошибкой
            });

        // Ждем завершения Task
        await taskCompletionSource.Task;
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
    /// <param name="playerName"> Имя игрока </param>
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
    public async Task PutPlayerResources(string playerName, int playerIron, int playerEnergy, int playerFood, int playerCrioCrystal)
    {
        // Создаем объект PlayerData
        PlayerData playerData = new PlayerData()
        {
            name = playerName,
            resources = new PlayerResources()
            {
                Iron = playerIron,
                Energy = playerEnergy,
                Food = playerFood,
                CryoCrystal = playerCrioCrystal
            }
        };

        // Преобразуем объект в JSON
        string json = JsonUtility.ToJson(playerData, true);
        
        Debug.Log(json);

        // Формируем URL для PUT-запроса
        string URL = Requests.PutPlayerURL(playerName);

        // Создаем TaskCompletionSource для ожидания завершения запроса
        var taskCompletionSource = new TaskCompletionSource<bool>();

        // Выполняем PUT-запрос
        HTTPRequests.Instance.Put(URL, json,
            onSuccess: response =>
            {
                Debug.Log("Ресурсы персонажа успешно обновлены");
                Debug.Log(response);
                taskCompletionSource.SetResult(true); // Завершаем Task успешным результатом
            },
            onError: error =>
            {
                Debug.LogError($"Ошибка при обновлении ресурсов персонажа: {error}");
                taskCompletionSource.SetException(new Exception(error)); // Завершаем Task с ошибкой
            });

        // Ждем завершения Task
        await taskCompletionSource.Task;
    }


    /// <summary>
    /// Удаление игрока
    /// </summary>
    /// <param name="playerName"></param>
    public async Task DeletePlayer(string playerName)
    {
        // Формируем URL для удаления игрока
        string URL = Requests.DeletePlayerURL(playerName);

        // Создаем TaskCompletionSource для обработки результата запроса
        var taskCompletionSource = new TaskCompletionSource<bool>();

        // Выполняем DELETE-запрос
        HTTPRequests.Instance.Delete(URL,
            onSuccess: response =>
            {
                Debug.Log("Персонаж успешно удален");
                taskCompletionSource.SetResult(true); // Завершаем Task успешным результатом
            },
            onError: error =>
            {
                Debug.LogError($"Ошибка при удалении персонажа: {error}");
                taskCompletionSource.SetException(new Exception(error)); // Завершаем Task с ошибкой
            });

        // Ждем завершения Task
        await taskCompletionSource.Task;
    }


    public void CreatePlayerLog(string comment, string playerName, string changedIron, string changedEnergy, string changedFood, string changedCrioCrystal)
    {
        
    }

    public PlayerLog GetPlayerLogs()
    {
        PlayerLog playerLog = new PlayerLog();
        return playerLog;
    }

    /// <summary>
    /// Создает магазин
    /// </summary>
    /// <param name="playerName"> имя игрока </param>
    /// <param name="shopName"> имя магазина </param>
    /// <param name="apiaryShop"> чертеж пасеки </param>
    /// <param name="honeyGunShop"> чертеж медопушки </param>
    /// <param name="mobileBaseShop"> чертеж мобильной базы </param>
    /// <param name="storageShop"> чертеж хранилища </param>
    /// <param name="residentialModuleShop"> чертеж жилого модуля </param>
    /// <param name="breadwinnerShop"> чертеж добытчика</param>
    /// <param name="pierShop"> чертеж пристани </param>
    /// <returns></returns>
    public async Task CreateShop(string playerName, string shopName, int apiaryShop, int honeyGunShop, int mobileBaseShop, int storageShop, int residentialModuleShop, int breadwinnerShop, int pierShop)
    {
        // Создаем объект ShopData
        ShopData shopData = new ShopData()
        {
            name = shopName,
            resources = new ShopResources()
            {
                ApiaryShop = apiaryShop,
                HoneyGunShop = honeyGunShop,
                MobileBaseShop = mobileBaseShop,
                StorageShop = storageShop,
                ResidentialModuleShop = residentialModuleShop,
                BreadwinnerShop = breadwinnerShop,
                PierShop = pierShop
            }
        };

        // Преобразуем в JSON
        string json = JsonUtility.ToJson(shopData, true);

        // Создаем TaskCompletionSource для ожидания ответа
        var taskCompletionSource = new TaskCompletionSource<bool>();

        // Выполняем POST-запрос
        HTTPRequests.Instance.Post(Requests.CreateShopURL(playerName), json, 
            onSuccess: response =>
            {
                Debug.Log("Магазин персонажа успешно создан");
                taskCompletionSource.SetResult(true); // Завершаем Task успешным результатом
            },
            onError: error =>
            {
                Debug.LogError($"Ошибка при создании Магазина персонажа: {error}");
                taskCompletionSource.SetException(new Exception(error)); // Завершаем Task с ошибкой
            });

        // Ждем завершения Task
        await taskCompletionSource.Task;
    }

    /// <summary>
    /// Получает Dictionary со списком магазинов игрока
    /// </summary>
    /// <param name="playerName"> имя игрока</param>
    /// <returns></returns>
    public async Task<Dictionary<string, ShopData>> GetShopsList(string playerName)
    {
        // Создаем TaskCompletionSource для обработки асинхронного ответа
        var taskCompletionSource = new TaskCompletionSource<Dictionary<string, ShopData>>();

        string URL = Requests.GetShopsURL(playerName);
        
        HTTPRequests.Instance.Get(URL,
            onSuccess: response =>
            {
                try
                {
                    // Парсим ответ
                    List<ShopData> shops = JsonUtility.FromJson<ShopsDataList>($"{{\"shops\":{response}}}").shops;

                    // Проверяем, есть ли игроки
                    var shopsDict = new Dictionary<string, ShopData>();
                    foreach (var shop in shops)
                    {
                        if (!shopsDict.ContainsKey(shop.name))
                        {
                            shopsDict[shop.name] = shop;
                        }
                    }
                    // Завершаем Task успешным результатом
                    taskCompletionSource.SetResult(shopsDict);
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
    /// Получение данных о ресурсах магазина 
    /// </summary>
    /// <param name="playerName"> Имя игрока </param>
    /// <param name="shopName"> Имя магазина </param>
    /// <returns></returns>
    public async Task<ShopResources> GetShopResources(string playerName, string shopName)
    {
        string URL = Requests.GetShopURL(playerName, shopName);

        // Создаем TaskCompletionSource для ожидания результата запроса
        var taskCompletionSource = new TaskCompletionSource<ShopResources>();

        HTTPRequests.Instance.Get(URL, 
            onSuccess: response =>
            {
                Debug.Log("Данные о ресурсах магазина успешно получены");
                ShopData shopData = JsonUtility.FromJson<ShopData>(response);
                taskCompletionSource.SetResult(shopData.resources); // Устанавливаем результат
            },
            onError: error =>
            {
                Debug.LogError("Возникла ошибка при получении данных о магазине: " + error);
                taskCompletionSource.SetException(new Exception(error)); // Устанавливаем исключение
            });

        // Ждем завершения Task и возвращаем результат
        return await taskCompletionSource.Task;
    }

    /// <summary>
    /// Обновляет ресурсы в магазине
    /// </summary>
    /// <param name="playerName"> имя игрока </param>
    /// <param name="shopName"> имя магазина </param>
    /// <param name="apiaryShop"> чертеж пасеки </param>
    /// <param name="honeyGunShop"> чертеж медопушки </param>
    /// <param name="mobileBaseShop"> чертеж мобильной базы </param>
    /// <param name="storageShop"> чертеж хранилища </param>
    /// <param name="residentialModuleShop"> чертеж жилого модуля </param>
    /// <param name="breadwinnerShop"> чертеж добытчика</param>
    /// <param name="pierShop"> чертеж пристани </param>
    /// <returns></returns>
    public async Task PutShopResources(string playerName, string shopName, int apiaryShop, int honeyGunShop, int mobileBaseShop, int storageShop, int residentialModuleShop, int breadwinnerShop, int pierShop)
    {
        // Создаем объект ShopData
        ShopData shopData = new ShopData()
        {
            name = shopName,
            resources = new ShopResources()
            {
                ApiaryShop = apiaryShop,
                HoneyGunShop = honeyGunShop,
                MobileBaseShop = mobileBaseShop,
                StorageShop = storageShop,
                ResidentialModuleShop = residentialModuleShop,
                BreadwinnerShop = breadwinnerShop,
                PierShop = pierShop
            }
        };

        // Преобразуем в JSON
        string json = JsonUtility.ToJson(shopData, true);

        // Создаем TaskCompletionSource для ожидания ответа
        var taskCompletionSource = new TaskCompletionSource<bool>();

        // Выполняем PUT-запрос
        HTTPRequests.Instance.Put(Requests.PutShopURL(playerName, shopName), json, 
            onSuccess: response =>
            {
                Debug.Log("Магазин персонажа успешно обновлен");
                taskCompletionSource.SetResult(true); // Завершаем Task успешным результатом
            },
            onError: error =>
            {
                Debug.LogError($"Ошибка при обновлении Магазина персонажа: {error}");
                taskCompletionSource.SetException(new Exception(error)); // Завершаем Task с ошибкой
            });

        // Ждем завершения Task
        await taskCompletionSource.Task;
    }

    /// <summary>
    /// Удаляет магазин определенного игрока
    /// </summary>
    /// <param name="playerName"> имя игрока </param>
    /// <param name="shopName"> имя магазина </param>
    /// <returns></returns>
    public async Task DeleteShop(string playerName, string shopName)
    {
        // Формируем URL для удаления игрока
        string URL = Requests.DeleteShopURL(playerName, shopName);

        // Создаем TaskCompletionSource для обработки результата запроса
        var taskCompletionSource = new TaskCompletionSource<bool>();

        // Выполняем DELETE-запрос
        HTTPRequests.Instance.Delete(URL,
            onSuccess: response =>
            {
                Debug.Log("Магазин успешно удален");
                taskCompletionSource.SetResult(true); // Завершаем Task успешным результатом
            },
            onError: error =>
            {
                Debug.LogError($"Ошибка при удалении магазина: {error}");
                taskCompletionSource.SetException(new Exception(error)); // Завершаем Task с ошибкой
            });

        // Ждем завершения Task
        await taskCompletionSource.Task;
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
