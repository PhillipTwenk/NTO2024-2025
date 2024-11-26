using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameEvent OpenBuildingPanelEvent;
    public GameEvent CloseBuildingPanelEvent;
    public GameEvent StartPlacingBuildEvent;
    public GameEvent EndPlacingBuildEvent;
    public GameEvent OpenBarterMenuEvent;
    public GameEvent CloseBarterMenuEvent;
    public GameEvent OpenHiringWorkersPanelEvent;
    public GameEvent CloseHiringWorkersPanelEvent;
    public GameEvent CloseTabletMenuEvent;

    [SerializeField] private Transform NewPlanPosition;
    [SerializeField] private Transform ContentPanel;

    [SerializeField] private List<Plan> plansArray;

    private bool IsOpenBuildingPanel;
    
    public static UIManager Instance { get; set; }

    public void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        IsOpenBuildingPanel = true;
    }

    private void Update()
    {
        if (Input.GetButtonDown("OpenBuildingPanel") && IsOpenBuildingPanel)
        {
            Debug.Log("Открыта панель строительства");
            OpenBuildingPanelEvent.TriggerEvent();
            IsOpenBuildingPanel = false;
            return;
        }
        if (Input.GetButtonDown("OpenBuildingPanel") && !IsOpenBuildingPanel)
        {
            Debug.Log("Закрыта панель строительства");
            EndPlacingBuildEvent.TriggerEvent();
            CloseBuildingPanelEvent.TriggerEvent();
            Destroy(BuildingManager.Instance.MouseIndicator);
            IsOpenBuildingPanel = true;
            return;
        }
    }

    /// <summary>
    /// Добавляет возможность строить новое здание после покупки нового чертежа
    /// </summary>
    public void AddNewPlanInPanel(Plan plan)
    {
        GameObject newPlanGameObject = Instantiate(plan.PlanPrefab, NewPlanPosition);

        newPlanGameObject.transform.SetParent(ContentPanel);
        
        TextMeshProUGUI titleTMPro = newPlanGameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI descriptionTMPro = newPlanGameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        Image sprite = newPlanGameObject.transform.GetChild(2).GetComponent<Image>();
        TextMeshProUGUI durabilityTMPro = newPlanGameObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI energyHoneyConsumptionTMPro = newPlanGameObject.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI resourceProductionTMPro = newPlanGameObject.transform.GetChild(5).GetComponent<TextMeshProUGUI>();

        titleTMPro.text = plan.Title;
        descriptionTMPro.text = plan.Description;
        sprite.sprite = plan.planSprite;
        durabilityTMPro.text = $"- Прочность: {plan.durability}";
        energyHoneyConsumptionTMPro.text = $"- Потребляет: {plan.energyHoneyConsumption}";
        resourceProductionTMPro.text = $"- Производит: {plan.resourceProduction}";

        Button ButtonComponent = newPlanGameObject.GetComponent<Button>();
        ButtonComponent.onClick.AddListener(() => StartPlacingNewBuilding(plan));
    }

    /// <summary>
    /// Инициализация панели строительства
    /// </summary>
    public async void InitializeBuildingPanel()
    {
        string playerName = UIManagerLocation.WhichPlayerCreate.Name;
        string shopName = $"{playerName}'sShop";
        ShopResources shopResources = await APIManager.Instance.GetShopResources(playerName, shopName);

        if (shopResources.ApiaryShop == 0)
            AddNewPlanInPanel(plansArray[0]);
        if (shopResources.HoneyGunShop == 0)
            AddNewPlanInPanel(plansArray[1]);
        if (shopResources.MobileBaseShop == 0)
            AddNewPlanInPanel(plansArray[2]);
        if (shopResources.StorageShop == 0)
            AddNewPlanInPanel(plansArray[3]);
        if (shopResources.ResidentialModuleShop == 0)
            AddNewPlanInPanel(plansArray[4]);
        if (shopResources.BreadwinnerShop == 0)
            AddNewPlanInPanel(plansArray[5]);
        if (shopResources.PierShop == 0)
            AddNewPlanInPanel(plansArray[6]);

        LoadingCanvasController.Instance.LoadingCanvasNotTransparent.SetActive(false);
        
    }

    /// <summary>
    /// Нажатие на кнопку старта строительства
    /// Начинаем размещать строение на земле
    /// </summary>
    public void StartPlacingNewBuilding(Plan plan)
    {
        GameObject PlaseNewBuildingTrigger = Instantiate(plan.buildingSO.PrefabBeforeBuilding);
        BuildingManager.Instance.MouseIndicator = PlaseNewBuildingTrigger;
        BuildingManager.Instance.CurrentBuilding = plan.buildingSO.PrefabBuilding;
        StartPlacingBuildEvent.TriggerEvent();
    }

    /// <summary>
    /// Октрытие меню бартера
    /// </summary>
    public void OpenBarterMenu()
    {
        OpenBarterMenuEvent.TriggerEvent();
    }
    
    /// <summary>
    /// Закрытие меню бартера
    /// </summary>
    public void CloseBarterMenu()
    {
        CloseBarterMenuEvent.TriggerEvent();
    }
    
    /// <summary>
    /// Октрытие меню торговли медведями
    /// </summary>
    public void OpenHiringWorkersPanel()
    {
        OpenHiringWorkersPanelEvent.TriggerEvent();
    }
    
    /// <summary>
    /// Закрытие меню торговли медведями
    /// </summary>
    public void CloseHiringWorkersPanel()
    {
        CloseHiringWorkersPanelEvent.TriggerEvent();
    }
    
    /// <summary>
    /// закрываааааааает окошко юный колонизатор
    /// </summary>
    public void FunctionCloseTabletMenu()
    {
        CloseTabletMenuEvent.TriggerEvent();
    }
}
