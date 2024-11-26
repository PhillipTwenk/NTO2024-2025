using System;
using System.Collections;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    
    public static BuildingManager Instance { get; private set; }
    
    private bool IsBuildingActive;
    public bool CanBuilding;
    
    [SerializeField] private Camera MainCamera;
    private Vector3 lastPosition;
    
    public GameObject MouseIndicator;
    [SerializeField] private LayerMask placementLayerMask;

    [SerializeField] private float YplaceVector;

    public GameObject CurrentBuilding;

    [SerializeField] private float awaitValueBuild;

    [SerializeField] private GameObject TextNotEnoughResource;

    [SerializeField] private GameEvent UpdateResourcesEvent;

    public void StartPlacingBuild() => IsBuildingActive = true;
    public void EndPlacingBuild() => IsBuildingActive = false;

    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
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
        LoadingCanvasController.Instance.LoadingCanvasTransparent.SetActive(true);

        
        Building buildingPrefabSO = CurrentBuilding.gameObject.transform.GetChild(0).GetComponent<BuildingData>().buildingTypeSO;
        int priceBuilding = buildingPrefabSO.priceBuilding;

        string playerName = UIManagerLocation.WhichPlayerCreate.Name;
        PlayerResources playerResources =
            await APIManager.Instance.GetPlayerResources(playerName);
        if (playerResources.Iron >= priceBuilding && PlansInShopControl.BaseLevel >= buildingPrefabSO.MBLevelForBuidlingthisIron)
        {
            await APIManager.Instance.PutPlayerResources(playerName, playerResources.Iron - priceBuilding,
                playerResources.Energy, playerResources.Food, playerResources.CryoCrystal);
            
            MouseIndicator.transform.position = new Vector3(mousePosition.x, YplaceVector, mousePosition.z);
        
            GameObject newBuildingObject = Instantiate(CurrentBuilding);
            newBuildingObject.transform.position = MouseIndicator.transform.position;
        
            Destroy(MouseIndicator);
        
            CurrentBuilding = null;
            IsBuildingActive = false;
            CanBuilding = true;

            BuildingData buildingData = newBuildingObject.transform.GetChild(0).GetComponent<BuildingData>();
            PlayerSaveData pLayerSaveData = UIManagerLocation.Instance.WhichPlayerDataUse();
            pLayerSaveData.playerBuildings.Add(buildingData.buildingTypeSO.PrefabBuilding);
        
            TransformData transformData = new TransformData(newBuildingObject.transform);
            pLayerSaveData.buildingsTransform.Add(transformData);
        
            BuildingSaveData buildingSaveData = new BuildingSaveData(buildingData);
            pLayerSaveData.BuildingDatas.Add(buildingSaveData);

            buildingData.SaveListIndex = pLayerSaveData.BuildingDatas.IndexOf(buildingSaveData);
            //StartCoroutine(TimerBuildingCoroutine(awaitValueBuild));
        }
        else
        {
            TextNotEnoughResource.SetActive(true);
            Utility.Invoke(this, () => TextNotEnoughResource.SetActive(false), 3f);
        }
        
        UpdateResourcesEvent.TriggerEvent();
        
        LoadingCanvasController.Instance.LoadingCanvasTransparent.SetActive(false);
    }
    // private IEnumerator TimerBuildingCoroutine(float await)
    // {
    //     
    // } 
}

