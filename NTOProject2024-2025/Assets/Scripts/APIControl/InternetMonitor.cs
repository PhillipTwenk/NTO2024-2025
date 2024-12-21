using UnityEngine;
using System;
using System.Collections;

public class InternetMonitor : MonoBehaviour
{
    public static event Action OnInternetConnected; // Событие для подключения интернета
    private bool isInternetAvailable; // Текущее состояние интернета
    public GameObject NoInternetUI;

    private void Start()
    {
        // Инициализация состояния интернета
        isInternetAvailable = Application.internetReachability != NetworkReachability.NotReachable;
        NoInternetUI.SetActive(!isInternetAvailable);
        
        StartCoroutine(CheckInternetConnection());
    }

    private IEnumerator CheckInternetConnection()
    {
        while (true)
        {
            // Проверяем доступность интернета
            bool currentInternetState = Application.internetReachability != NetworkReachability.NotReachable;

            if (currentInternetState && !isInternetAvailable)
            {
                Debug.Log("Интернет подключен!");
                
                NoInternetUI.SetActive(false);
                isInternetAvailable = true;

                // Вызываем событие подключения интернета
                OnInternetConnected?.Invoke();
            }
            else if (!currentInternetState && isInternetAvailable)
            {      
                Debug.Log("Интернет отключен!");
                
                NoInternetUI.SetActive(true);
                isInternetAvailable = false;
            }

            // Проверка каждые 2 секунды
            yield return new WaitForSeconds(2f);
        }
    }
}