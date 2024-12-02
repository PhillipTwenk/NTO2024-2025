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

    [SerializeField] private TextMeshProUGUI HintPanel;
    [SerializeField] private TextMeshProUGUI ButtonUpgradeTextPanel;
    [TextArea] [SerializeField] private string TextNotEnoughtResources;
    [TextArea] [SerializeField] private string TextNotEnoughtBaseLevel;
    [TextArea] [SerializeField] private string UpgradeLevelBuildingInformation;
    [TextArea] [SerializeField] private string TextNotCompleteConditionUpgradeMB;
    [TextArea] [SerializeField] private string TextCompleteUpgradeMobileBaseLevel;


    [SerializeField] private float TimeHint;

    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject point;
    [SerializeField] private GameObject ButtonUpgrade;

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
        ButtonUpgrade.SetActive(false);
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
        Debug.Log(9012);
        if (!BuildingManager.Instance.ProcessWorkerBuildingActive)
        {
            Debug.Log(0987654321);
            IsPanelActive = true;
        
            point.SetActive(true);
            panel.SetActive(true);

            if (buildingSO.priceUpgrade > 0 && buildingSO.Level(BaseUpgradeConditionManager.CurrentBaseLevel) <= buildingSO.MaxLevelThisBuilding)
            {
                ButtonUpgrade.SetActive(true);
                ButtonUpgradeTextPanel.text = buildingSO.priceUpgrade.ToString();
            }
            else
            {
                ButtonUpgrade.SetActive(false);
            }

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

            if (buildingSO.Production(buildingData.Level).resources.Count != 0)
            {
                //Формирование строки после "Производит:" на панели
                string productionTextOutput = "Производит:";
                int iP = 0;
                List<int> listIndexSAProduction = buildingSO.Production(buildingData.Level).SpriteAssetsUsingIndex;
                List<int> resourcesValuesProduction = buildingSO.Production(buildingData.Level).resources;
                foreach (var resource in resourcesValuesProduction)
                {
                    if (iP >= 1)
                    {
                        productionTextOutput += $"+ {resource}   <sprite={listIndexSAProduction[iP]}>";
                    }
                    else
                    {
                        productionTextOutput += $" {resource}   <sprite={listIndexSAProduction[iP]}>";
                    }
                    iP++;
                }
                Production.text = productionTextOutput;
            }
            else
            {
                Production.gameObject.SetActive(false);
            }
            
            //Формирование строки о трате энергомеда
            HoneyConsumption.text = $"Тратит: {Convert.ToString(buildingSO.EnergyHoneyConsumpiton(buildingData.Level))} <sprite=0>";

            if (buildingSO.StorageLimit(buildingData.Level).resources.Count != 0)
            {
                //Формирование строки после "Количество ресурсов:" на панели
                string storageTextOutput = "Количество ресурсов:";
                int iS = 0;
                List<int> listIndexSAStorage = buildingSO.StorageLimit(buildingData.Level).SpriteAssetsUsingIndex;
                List<int> resourcesValuesStorage = buildingSO.StorageLimit(buildingData.Level).resources;
                foreach (var resource in resourcesValuesStorage)
                {
                    if (iS >= 1)
                    {
                        storageTextOutput += $"+ {buildingData.Storage[iS]} / {resource}   <sprite={listIndexSAStorage[iS]}>";
                    }
                    else
                    {
                        storageTextOutput += $" {buildingData.Storage[iS]} / {resource}   <sprite={listIndexSAStorage[iS]}>"; 
                    }
                    iS++;
                }
                Storage.text = storageTextOutput;
            }
            else
            {
                Storage.gameObject.SetActive(false);
            }

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

    /// <summary>
    /// Улучшение здания
    /// </summary>
    public async void UpgradeBuilding()
    {
        LoadingCanvasController.Instance.LoadingCanvasTransparent.SetActive(true);

        string playerName = UIManagerLocation.WhichPlayerCreate.Name;
        PlayerResources playerResources = await APIManager.Instance.GetPlayerResources(playerName);

        int priceUpgrade = buildingSO.priceUpgrade;
        int BaseLevel = BaseUpgradeConditionManager.CurrentBaseLevel;

        if (buildingData.Title != "Мобильная база")
        {
            if (playerResources.Iron >= priceUpgrade)
            {
                if (BaseLevel >= buildingSO.MBLevelForUpgradethisIron)
                {
                    await APIManager.Instance.PutPlayerResources(playerName, playerResources.Iron - priceUpgrade,
                        playerResources.Energy, playerResources.Food, playerResources.CryoCrystal);

                    buildingData.Level += 1;
                    buildingData.Durability = buildingSO.Durability(BaseLevel);
                    buildingData.HoneyConsumption = buildingSO.EnergyHoneyConsumpiton(BaseLevel);

                    if (buildingData.Production.Count > 0)
                    {
                        buildingData.Production = buildingSO.Production(BaseLevel).resources;
                    }

                    PlayerSaveData playerSaveData = UIManagerLocation.Instance.WhichPlayerDataUse();
                    
                    BuildingSaveData buildingSaveData = new BuildingSaveData(buildingData);
                    playerSaveData.BuildingDatas[buildingData.SaveListIndex] = buildingSaveData;
                    
                    OnHintPanel(UpgradeLevelBuildingInformation);
                }
                else
                {
                    OnHintPanel(TextNotEnoughtBaseLevel);
                }
            }
            else
            {
                OnHintPanel(TextNotEnoughtResources);
            }
        }
        else
        {
            List<string> ImprovementReport = BaseUpgradeConditionManager.Instance.CanUpgradeMobileBase(playerResources);
            if (ImprovementReport[0] == BaseUpgradeConditionManager.Instance.SuccesUpgradeText)
            {
                await APIManager.Instance.PutPlayerResources(playerName, playerResources.Iron - priceUpgrade,
                        playerResources.Energy, playerResources.Food, playerResources.CryoCrystal);
                buildingData.Level += 1;
                buildingData.Durability = buildingSO.Durability(BaseLevel);
                buildingData.HoneyConsumption = buildingSO.EnergyHoneyConsumpiton(BaseLevel);

                BaseUpgradeConditionManager.CurrentBaseLevel = buildingData.Level;

                PlayerSaveData playerSaveData = UIManagerLocation.Instance.WhichPlayerDataUse();
                    
                BuildingSaveData buildingSaveData = new BuildingSaveData(buildingData);
                playerSaveData.BuildingDatas[buildingData.SaveListIndex] = buildingSaveData;


                OnHintPanel(TextCompleteUpgradeMobileBaseLevel);
            }
            else
            {
                string UnsuccessfullReportText = $"";
                foreach (var report in ImprovementReport)
                {
                    UnsuccessfullReportText += $"\n- {report} ";
                }
                
                OnHintPanel(TextNotCompleteConditionUpgradeMB + UnsuccessfullReportText);
            }
        }
        
        LoadingCanvasController.Instance.LoadingCanvasTransparent.SetActive(false);
    }

    private void OnHintPanel(string Text)
    {
        HintPanel.transform.parent.gameObject.SetActive(true);
        Utility.Invoke(this, () => HintPanel.transform.parent.gameObject.SetActive(false), TimeHint);
        HintPanel.text = Text;
    }
}
