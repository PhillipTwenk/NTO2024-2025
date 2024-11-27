using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class BuildingManager : MonoBehaviour
{
    
    public static BuildingManager Instance { get; private set; }
    
    [TextArea] [SerializeField] private string HintNotEnoughtResourcesText;
    [TextArea] [SerializeField] private string HintNotEnoughtLevelBaseText;
    [TextArea] [SerializeField] private string HintNotFreeWorkersText;
    [SerializeField] private float TimeHint;
    [SerializeField] private GameObject TextNotEnoughResource;
    [SerializeField] private TextMeshProUGUI TextHintTMPRoUGUI;

    public bool IsBuildingActive;
    public bool CanBuilding;
    public bool ProcessWorkerBuildingActive;
    
    [SerializeField] private Camera MainCamera;
    private Vector3 lastPosition;
    
    public GameObject MouseIndicator;
    [SerializeField] private LayerMask placementLayerMask;

    [SerializeField] private float YplaceVector;

    public GameObject CurrentBuilding;

    [SerializeField] private float awaitValueBuild;

    [SerializeField] private GameEvent UpdateResourcesEvent;

    public void StartPlacingBuild() => IsBuildingActive = true;
    public void EndPlacingBuild() => IsBuildingActive = false;

    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        ProcessWorkerBuildingActive = false;
        IsBuildingActive = false;
        CanBuilding = true;
    }

    
    private void Update()
    {
        if (IsBuildingActive)
        {
            Vector3 mousePosition = GetSelectedMapPosition();
            MouseIndicator.transform.position = new Vector3(mousePosition.x, mousePosition.y + 0.5f, mousePosition.z);
            
            if (Input.GetMouseButtonDown(0) && CanBuilding)
            {
                PlaceBuilding(mousePosition);
            }
        }
    }

    /// <summary>
    /// Возвращает позицию мыши
    /// </summary>
    /// <returns></returns>
    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = MainCamera.nearClipPlane;
        Ray ray = MainCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayerMask))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }
    
    /// <summary>
    /// Размещение строения
    /// </summary>
    /// <param name="mousePosition"></param>
    public async void PlaceBuilding(Vector3 mousePosition)
    {
        ProcessWorkerBuildingActive = true;
        LoadingCanvasController.Instance.LoadingCanvasTransparent.SetActive(true);

        
        Building buildingPrefabSO = CurrentBuilding.gameObject.transform.GetChild(0).GetComponent<BuildingData>().buildingTypeSO;
        int priceBuilding = buildingPrefabSO.priceBuilding;

        string playerName = UIManagerLocation.WhichPlayerCreate.Name;
        PlayerResources playerResources =
            await APIManager.Instance.GetPlayerResources(playerName);
        if (playerResources.Iron >= priceBuilding)
        {
            if(PlansInShopControl.BaseLevel >= buildingPrefabSO.MBLevelForBuidlingthisIron)
            {
                int CNoW = WorkersInterBuildingControl.Instance.CurrentValueOfWorkers;
                int MVoW = WorkersInterBuildingControl.Instance.MaxValueOfWorkers;
                int AW = WorkersInterBuildingControl.Instance.NumberOfActiveWorkers;
                if(CNoW <= MVoW && AW < CNoW)
                {
                    await APIManager.Instance.PutPlayerResources(playerName, playerResources.Iron - priceBuilding,
                playerResources.Energy, playerResources.Food, playerResources.CryoCrystal);
            
                    //Создаем новое здание, устанавливаем его позицию и удаляем триггер для строительства
                    MouseIndicator.transform.position = new Vector3(mousePosition.x, YplaceVector, mousePosition.z);
                    GameObject newBuildingObject = Instantiate(CurrentBuilding);
                    newBuildingObject.transform.position = MouseIndicator.transform.position;
                    Destroy(MouseIndicator);
        
                    IsBuildingActive = false;
                    CurrentBuilding = null;
                    CanBuilding = true;

                    //Получение некорых данных о здании
                    GameObject ComponentContainingBuilding = newBuildingObject.transform.GetChild(0).gameObject;
                    BuildingData buildingData = ComponentContainingBuilding.GetComponent<BuildingData>();

                    LoadingCanvasController.Instance.LoadingCanvasTransparent.SetActive(false);
                    
                    //Ожидаем прибытия рабочего 
                    await WorkersInterBuildingControl.Instance.SendWorkerToBuilding(true, buildingData);
                    
                    //Ожидаем завершения его строительства
                    await WorkersInterBuildingControl.Instance.WorkerEndWork(buildingData);


                    LoadingCanvasController.Instance.LoadingCanvasTransparent.SetActive(true);
                    //Сохранение данных здания в SO сохранения
                    PlayerSaveData pLayerSaveData = UIManagerLocation.Instance.WhichPlayerDataUse();
                    pLayerSaveData.playerBuildings.Add(buildingData.buildingTypeSO.PrefabBuilding);
        
                    TransformData transformData = new TransformData(newBuildingObject.transform);
                    pLayerSaveData.buildingsTransform.Add(transformData);
        
                    BuildingSaveData buildingSaveData = new BuildingSaveData(buildingData);
                    pLayerSaveData.BuildingDatas.Add(buildingSaveData);

                    buildingData.SaveListIndex = pLayerSaveData.BuildingDatas.IndexOf(buildingSaveData);

                    if (ComponentContainingBuilding.GetComponent<ThisBuildingWorkersControl>())
                    {
                        ThisBuildingWorkersControl thisBuildingWorkersControl = ComponentContainingBuilding.GetComponent<ThisBuildingWorkersControl>();
                        WorkersContolSaveData worlersSaveData = new WorkersContolSaveData(thisBuildingWorkersControl);
                        pLayerSaveData.BuildingWorkersInformationList.Add(worlersSaveData);

                        WorkersInterBuildingControl.Instance.AddNewBuilding(thisBuildingWorkersControl);
                    }
                    ProcessWorkerBuildingActive = false;
                }
                else
                {
                    TextNotEnoughResource.SetActive(true);
                    Utility.Invoke(this, () => TextNotEnoughResource.SetActive(false), TimeHint);
                    TextHintTMPRoUGUI.text = HintNotFreeWorkersText;
                }
            }
            else
            {
                TextNotEnoughResource.SetActive(true);
                Utility.Invoke(this, () => TextNotEnoughResource.SetActive(false), TimeHint);
                TextHintTMPRoUGUI.text = HintNotEnoughtLevelBaseText;
            }
        }
        else
        {
            TextNotEnoughResource.SetActive(true);
            Utility.Invoke(this, () => TextNotEnoughResource.SetActive(false), TimeHint);
            TextHintTMPRoUGUI.text = HintNotEnoughtResourcesText;
        }
        
        UpdateResourcesEvent.TriggerEvent();
        LoadingCanvasController.Instance.LoadingCanvasTransparent.SetActive(false);
    }
}

