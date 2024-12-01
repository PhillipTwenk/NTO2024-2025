using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionBuildingController : MonoBehaviour
{
    private bool CanPutE;
    [SerializeField] private bool PossiblityPutEInThisBuilding;
    [SerializeField] private GameEvent OpenDescriptionPanel;
    private BuildingData _buildingData;

    [SerializeField] private UnityEvent InteractionEvent;
    
    public GameEvent OpenBarterMenuEvent;
    public GameEvent CloseBarterMenuEvent;

    public GameObject Texthint;

    [SerializeField] private List<Transform> PointsOfBuildings;

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
            Texthint.SetActive(true);
        }
        if(other.gameObject.CompareTag("Worker") && WorkersInterBuildingControl.CurrentBuilding.Title == GetComponent<BuildingData>().Title){
            if (BuildingManager.Instance.ProcessWorkerBuildingActive)
            {
                if (!other.gameObject.GetComponent<WorkerMovementController>().ReadyForWork)
                {
                    Debug.Log(1);
                    WorkersInterBuildingControl.CurrentBuilding.gameObject.GetComponent<ThisBuildingWorkersControl>()
                        .CurrentNumberWorkersInThisBuilding += 1;
                    WorkersInterBuildingControl.CurrentBuilding.gameObject.GetComponent<ThisBuildingWorkersControl>()
                        .NumberOfActiveWorkersInThisBuilding -= 1;
                    WorkersInterBuildingControl.Instance.CurrentValueOfWorkers += 1;
                    WorkersInterBuildingControl.Instance.NumberOfActiveWorkers -= 1;

                    BuildingManager.Instance.ProcessWorkerBuildingActive = false;

                    Destroy(other.gameObject);
                }
                else
                {
                    Debug.Log(WorkersInterBuildingControl.CurrentBuilding.Title);
                    Debug.Log("Рабочий добрался, начинает строить здание");
                    WorkersInterBuildingControl.Instance.NotifyWorkerArrival();

                    GameObject worker = other.gameObject;
                    WorkersInterBuildingControl.Instance.StartAnimationBuilding(worker.transform, worker.GetComponent<Animator>(), worker.GetComponent<WorkerMovementController>(), PointsOfBuildings, GetComponent<BuildingData>());
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
    private void OnMouseDown()
    {
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
