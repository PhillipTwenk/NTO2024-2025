using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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

    public static string GetShopLog(string playerName, string shopName) => $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/{playerName}/shops/{shopName}/logs/";

    public static string GetLogGame = $"https://2025.nti-gamedev.ru/api/games/{UUID}/logs/";
}

/// <summary>
/// Класс - менеджер, предоставляющий методы для работы с запросами
/// Получить ответ можно через onSucces -> responce
/// </summary>
public class HTTPRequests: MonoBehaviour
 { 
     public static HTTPRequests Instance { get; private set; }
     
     private void Awake()
     {
         Instance = this;
         
         // //Test
         // PlayerData pd = new PlayerData()
         // {
         //     name = "TestPlayerFromScript",
         //     resources = new PlayerResources()
         //     {
         //         Iron = 10,
         //         Energy = 10,
         //         Food = 10,
         //         CrioCrystal = 10
         //     }
         // };
         //
         //
         // string json = JsonUtility.ToJson(pd, true);
         //
         //
         // HTTPRequests.Instance.Post(Requests.CreatePlayerURL, json,
         //     onSuccess: responce =>
         //     {
         //         Debug.Log(responce);
         //     },
         //     onError: responce =>
         //     {
         //         Debug.Log("Error");
         //     });
         // //
     }
     
     /// <summary>
    /// Выполнить GET запрос
    /// </summary>
    public void Get(string url, Action<string> onSuccess, Action<string> onError = null)
    {
        StartCoroutine(SendRequest(url, UnityWebRequest.kHttpVerbGET, null, onSuccess, onError));
    }

    /// <summary>
    /// Выполнить POST запрос
    /// </summary>
    public void Post(string url, string jsonData, Action<string> onSuccess, Action<string> onError = null)
    {
        StartCoroutine(SendRequest(url, UnityWebRequest.kHttpVerbPOST, jsonData, onSuccess, onError));
    }

    /// <summary>
    /// Выполнить PUT запрос
    /// </summary>
    public void Put(string url, string jsonData, Action<string> onSuccess, Action<string> onError = null)
    {
        StartCoroutine(SendRequest(url, UnityWebRequest.kHttpVerbPUT, jsonData, onSuccess, onError));
    }

    /// <summary>
    /// Выполнить DELETE запрос
    /// </summary>
    public void Delete(string url, Action<string> onSuccess, Action<string> onError = null)
    {
        StartCoroutine(SendRequest(url, UnityWebRequest.kHttpVerbDELETE, null, onSuccess, onError));
    }

    /// <summary>
    /// Общая корутина для выполнения запросов
    /// </summary>
    private IEnumerator SendRequest(string url, string method, string jsonData, Action<string> onSuccess, Action<string> onError)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, method))
        {
            if (!string.IsNullOrEmpty(jsonData))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.SetRequestHeader("Content-Type", "application/json");
            }

            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(request.downloadHandler.text);
            }
            else
            {
                Debug.LogError($"Request Error: {request.error}, URL: {url}");
                onError?.Invoke(request.error);
            }
        }
    }
 }
