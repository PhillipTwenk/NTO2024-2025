using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
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

            if (request.result == UnityWebRequest.Result.Success && !InternetMonitor.IsOfflineMode)
            {
                onSuccess?.Invoke(request.downloadHandler.text);
            }
            else if (!InternetMonitor.IsOfflineMode)
            {
                Debug.LogError($"Request Error: {request.error}, URL: {url}");
                onError?.Invoke(request.error);
            }
            else if (InternetMonitor.IsOfflineMode)
            {
                Debug.LogError($"Offline mode on");
                onError?.Invoke(request.error);
            }
        }
    }
 }
