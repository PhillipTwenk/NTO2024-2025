using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class InteractionBuildingController : MonoBehaviour
{
    private bool CanPutE;
    [SerializeField] private bool PossiblityPutEInThisBuilding;
    
    [SerializeField] private GameEvent OpenDescriptionPanel;
    public GameEvent OpenBarterMenuEvent;
    public GameEvent CloseBarterMenuEvent;
    private BuildingData _buildingData;

    [Header("InteractionEvents")]
    [SerializeField] private UnityEvent InteractionEvent;
    [SerializeField] private UnityEvent TextOnEvent;
    

    public GameObject Texthint;

    public List<Transform> PointsOfBuildings;

    private void Start()
    {
        _buildingData = GetComponent<BuildingData>();
        CanPutE = false;
        if (_buildingData.Title == "Мобильная база")
        {
            Texthint.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("InteractionWithBuilding") && CanPutE)
        {
            InteractionEvent?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && PossiblityPutEInThisBuilding)
        {
            CanPutE = true;
            TextOnEvent?.Invoke();
            Texthint.SetActive(true);
        }
        if(other.gameObject.CompareTag("Worker"))
        {
            if (WorkersInterBuildingControl.CurrentBuilding != null && WorkersInterBuildingControl.CurrentBuilding.Title == GetComponent<BuildingData>().Title)
            {
                if (GetComponent<ThisBuildingWorkersControl>() && !other.gameObject.GetComponent<WorkerMovementController>().ReadyForWork && BuildingManager.Instance.ProcessWorkerBuildingActive)
                {
                    Debug.Log(1);
                    WorkersInterBuildingControl.CurrentBuilding.gameObject.GetComponent<ThisBuildingWorkersControl>()
                        .CurrentNumberWorkersInThisBuilding += 1;
                    WorkersInterBuildingControl.CurrentBuilding.gameObject.GetComponent<ThisBuildingWorkersControl>()
                        .NumberOfActiveWorkersInThisBuilding -= 1;
                    WorkersInterBuildingControl.Instance.CurrentValueOfWorkers += 1;
                    WorkersInterBuildingControl.Instance.NumberOfActiveWorkers -= 1;

                    Destroy(other.gameObject);

                    BuildingManager.Instance.ProcessWorkerBuildingActive = false;
                    return;
                }
                if (!_buildingData.IsThisBuilt && other.gameObject.GetComponent<WorkerMovementController>().ReadyForWork)
                {
                    WorkerMovementController movementController = other.gameObject.GetComponent<WorkerMovementController>();
                    movementController.WorkerPointOfDestination = null;
                    
                    other.transform.LookAt(WorkersInterBuildingControl.CurrentBuilding.transform);
                    
                    Animator animator = other.gameObject.GetComponent<Animator>();
                    animator.SetBool("Running", false);
                    animator.SetBool("Building", true);
                    animator.SetBool("Idle", false);
                    
                    Debug.Log(WorkersInterBuildingControl.CurrentBuilding.Title);
                    Debug.Log("Рабочий добрался, начинает строить здание");
                    WorkersInterBuildingControl.Instance.NotifyWorkerArrival();

                    GameObject worker = other.gameObject;
                    WorkersInterBuildingControl.Instance.StartAnimationBuilding(worker.GetComponent<Animator>(), worker.GetComponent<WorkerMovementController>(), GetComponent<BuildingData>());
                    return;
                }
            }
            if (other.gameObject.GetComponent<WorkerMovementController>().SelectedBuilding && other.gameObject.GetComponent<WorkerMovementController>().ReadyForWork)
            {
                if (GetComponent<ThisBuildingWorkersControl>())
                {
                    ThisBuildingWorkersControl thisBuildingWorkersControl = GetComponent<ThisBuildingWorkersControl>();
                    TextMeshPro text = Texthint.GetComponent<TextMeshPro>();
                    thisBuildingWorkersControl.CurrentNumberWorkersInThisBuilding += 1;
                    text.text = $"Нажмите E чтобы выгрузить одного рабочего ({thisBuildingWorkersControl.CurrentNumberWorkersInThisBuilding}/2)";
                    Destroy(other.gameObject.transform.parent.gameObject);
                    return;
                }else if (GetComponent<EnergyProduction>())
                {
                    EnergyProduction energyProduction = GetComponent<EnergyProduction>();
                    if (GetComponent<BuildingData>().Storage[0] == 0)
                    {
                        GetComponent<BuildingData>().Storage[0] += 1;
                        energyProduction.OnAddEnergy();
                    }
                }
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && PossiblityPutEInThisBuilding)
        {
            CanPutE = false;
            Texthint.SetActive(false);
        }
    }

    /// <summary>
    /// Нажатие на здание
    /// </summary>
    public void OnMouseDown()
    {
        Debug.Log(01874380492);
        AddTextToDescriptionPanel.buildingData = _buildingData;
        AddTextToDescriptionPanel.buildingTransform = gameObject.transform;
        AddTextToDescriptionPanel.buildingSO = _buildingData.buildingTypeSO;
        
        OpenDescriptionPanel.TriggerEvent();
    }
    
    /// <summary>
    /// Октрытие меню бартера
    /// Вызывается из ивента в InteractionBuildingController на скрипте здания
    /// Ивент слушает UIActiveControl
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
}
