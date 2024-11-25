using System;
using TMPro;
using UnityEngine;

public class AddTextToDescriptionPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Title;
    [SerializeField] private TextMeshProUGUI Level;
    [SerializeField] private TextMeshProUGUI Durability;
    [SerializeField] private TextMeshProUGUI Production;
    [SerializeField] private TextMeshProUGUI HoneyConsumption;
    [SerializeField] private TextMeshProUGUI Storage;

    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject point;

    public static BuildingData buildingData;
    public static Transform buildingTransform;
    public static Building buildingSO;

    [SerializeField] private Transform pointInPanelAngle1;
    [SerializeField] private Transform pointInPanelAngle2;
    [SerializeField] private Transform pointInPanelAngle3;
    [SerializeField] private Transform pointInPanelAngle4; 
    private bool IsPanelActive;

    private void Start()
    {
        panel.SetActive(false);
        point.SetActive(false);
        IsPanelActive = false; 
    }

    private void Update()
    {
        if (IsPanelActive)
        {
            ShowDescriptionPanel();
        }
    }

    /// <summary>
    /// Нажали на здание, открытие панели подробной информации
    /// </summary>
    /// <param name="building"></param>
    public void ShowDescriptionPanel()
    {
        IsPanelActive = true;
        
        point.SetActive(true);
        panel.SetActive(true);

        // Центр экрана
        float centerX = Screen.width / 2;
        float centerY = Screen.height / 2;

        // Экранные координаты объекта
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(buildingTransform.position);
            
        float offsetX = Screen.width * 0.15f; // 10% от ширины экрана
        float offsetY = Screen.height * 0.1f; // 5% от высоты экрана
        
        point.transform.position = screenPosition;
        
        // Определяем четверть
        if (screenPosition.x > centerX && screenPosition.y > centerY)
        {
            Debug.Log("Объект находится в первой четверти.");
            Vector3 panelPosition = screenPosition + new Vector3(-offsetX, -offsetY, 0);
            
            panel.transform.position = panelPosition;
        }
        else if (screenPosition.x < centerX && screenPosition.y > centerY)
        {
            Debug.Log("Объект находится во второй четверти.");
            Vector3 panelPosition = screenPosition + new Vector3(offsetX, -offsetY, 0);
            panel.transform.position = panelPosition;
        }
        else if (screenPosition.x < centerX && screenPosition.y < centerY)
        {
            Debug.Log("Объект находится в третьей четверти.");
            Vector3 panelPosition = screenPosition + new Vector3(offsetX, offsetY, 0);
            panel.transform.position = panelPosition;
            
        }
        else if (screenPosition.x > centerX && screenPosition.y < centerY)
        {
            Debug.Log("Объект находится в четвёртой четверти.");
            Vector3 panelPosition = screenPosition + new Vector3(-offsetX, offsetY, 0);
            panel.transform.position = panelPosition;
            
        }
        
        Title.text = buildingData.Title;
        Level.text = $"Уровень: {Convert.ToString(buildingData.Level)}";
        Durability.text = $"Прочность: {Convert.ToString(buildingData.Durability)}";
        Production.text = $"Производит: {Convert.ToString(buildingData.Production)}";
        HoneyConsumption.text = $"Тратит: {Convert.ToString(buildingData.HoneyConsumption)}";
        Storage.text = $"Количество ресурсов: {Convert.ToString(buildingData.Storage)} / {buildingSO.StorageLimit}";
        
        //DescriptionCanvas.renderMode = RenderMode.WorldSpace;
        //DescriptionCanvas.worldCamera = mainCamera;
    }

    /// <summary>
    /// Сокрытие панели подробного описания здания
    /// </summary>
    public void HideDescriptionPanel()
    {
        IsPanelActive = false;
        point.SetActive(false);
        panel.SetActive(false);
        //DescriptionCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        //DescriptionCanvas.worldCamera = null;
    }
}
