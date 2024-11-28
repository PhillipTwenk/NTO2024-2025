using System;
using System.Collections.Generic;
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

    [SerializeField] private GameEvent UpdateResourcesEvent;
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
        if (!BuildingManager.Instance.ProcessWorkerBuildingActive)
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
                //Debug.Log("Объект находится в первой четверти.");
                Vector3 panelPosition = screenPosition + new Vector3(-offsetX, -offsetY, 0);
                
                panel.transform.position = panelPosition;
            }
            else if (screenPosition.x < centerX && screenPosition.y > centerY)
            {
                //Debug.Log("Объект находится во второй четверти.");
                Vector3 panelPosition = screenPosition + new Vector3(offsetX, -offsetY, 0);
                panel.transform.position = panelPosition;
            }
            else if (screenPosition.x < centerX && screenPosition.y < centerY)
            {
                //Debug.Log("Объект находится в третьей четверти.");
                Vector3 panelPosition = screenPosition + new Vector3(offsetX, offsetY, 0);
                panel.transform.position = panelPosition;
                
            }
            else if (screenPosition.x > centerX && screenPosition.y < centerY)
            {
                //Debug.Log("Объект находится в четвёртой четверти.");
                Vector3 panelPosition = screenPosition + new Vector3(-offsetX, offsetY, 0);
                panel.transform.position = panelPosition;
                
            }
            
            Title.text = buildingData.Title;

            //Формирование строки об уровне здания
            Level.text = $"Уровень: {Convert.ToString(buildingData.Level)}";

            //Формирование строки о текущей прочности здания
            Durability.text = $"Прочность: {Convert.ToString(buildingData.Durability)} / {buildingSO.Durability(buildingData.Level)}";

            //Формирование строки после "Производит:" на панели
            string productionTextOutput = "Производит:";
            int iP = 0;
            List<int> listIndexSAProduction = buildingSO.Production(buildingData.Level).SpriteAssetsUsingIndex;
            List<int> resourcesValuesProduction = buildingSO.Production(buildingData.Level).resources;
            foreach (var resource in resourcesValuesProduction)
            {
                if (iP > 1)
                {
                    productionTextOutput += $"+ {resource} <sprite={listIndexSAProduction[iP]}>";
                }
                else
                {
                    productionTextOutput += $" {resource} <sprite={listIndexSAProduction[iP]}>";
                }
                iP++;
            }
            Production.text = productionTextOutput;

            //Формирование строки о трате энергомеда
            HoneyConsumption.text = $"Тратит: {Convert.ToString(buildingSO.EnergyHoneyConsumpiton(buildingData.Level))}";

            //Формирование строки после "Количество ресурсов:" на панели
            string storageTextOutput = "Количество ресурсов:";
            int iS = 0;
            List<int> listIndexSAStorage = buildingSO.StorageLimit(buildingData.Level).SpriteAssetsUsingIndex;
            List<int> resourcesValuesStorage = buildingSO.StorageLimit(buildingData.Level).resources;
            foreach (var resource in resourcesValuesStorage)
            {
                if (iS > 1)
                {
                    productionTextOutput += $"+ {buildingData.Storage} / {resource} <sprite={listIndexSAStorage[iS]}>";
                }
                else
                {
                    productionTextOutput += $" {buildingData.Storage} / {resource} <sprite={listIndexSAStorage[iS]}>";
                }
                iS++;
            }
            Storage.text = storageTextOutput;
            
            //DescriptionCanvas.renderMode = RenderMode.WorldSpace;
            //DescriptionCanvas.worldCamera = mainCamera;
        }
        
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

    /// <summary>
    /// Разрушение здания
    /// </summary>
    public async void DestroyBuilding()
    {
        LoadingCanvasController.Instance.LoadingCanvasTransparent.SetActive(true);

        HideDescriptionPanel();
        
        string playerName = UIManagerLocation.WhichPlayerCreate.Name;
        PlayerResources playerResources = await APIManager.Instance.GetPlayerResources(playerName);
        
        GameObject building = buildingTransform.gameObject;
        Building buildingSO = building.GetComponent<BuildingData>().buildingTypeSO;

        int NewIron = buildingSO.priceBuilding / 2;

        await APIManager.Instance.PutPlayerResources(playerName, playerResources.Iron + NewIron, playerResources.Energy, playerResources.Food,
            playerResources.CryoCrystal);
        
        PlayerSaveData playerSaveData = UIManagerLocation.Instance.WhichPlayerDataUse();
        
        playerSaveData.DeleteBuilding(building);
        
        UpdateResourcesEvent.TriggerEvent();

        LoadingCanvasController.Instance.LoadingCanvasTransparent.SetActive(false);
    }
}
