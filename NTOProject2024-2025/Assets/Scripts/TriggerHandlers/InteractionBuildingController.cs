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
        // Если у здания можно нажать на Е, то при нажатии вызываем ивент, содержащий функционал здания
        if (Input.GetButtonDown("InteractionWithBuilding") && CanPutE)
        {
            InteractionEvent?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Если игрок около здания, вызываем подсказку о нажатии на Е и позволяем использование функционала
        if (other.gameObject.CompareTag("Player") && PossiblityPutEInThisBuilding)
        {
            if (GetComponent<BuildingData>().IsThisBuilt)
            {
                CanPutE = true;
                TextOnEvent?.Invoke();
                Texthint.SetActive(true);
            }
        }
        
        // Если рабочий около здания
        if(other.gameObject.CompareTag("Worker"))
        {
            // Если данное здание является текущим у данного рабочего
            if (WorkersInterBuildingControl.CurrentBuilding == gameObject.GetComponent<BuildingData>() && WorkersInterBuildingControl.CurrentBuilding.Title == GetComponent<BuildingData>().Title)
            {
                
                // Если данное здание может содержать рабочих, и при этом рабочий не занят постройкой здания + здание вообще построено
                if (GetComponent<ThisBuildingWorkersControl>() && !other.gameObject.GetComponent<WorkerMovementController>().ReadyForWork && BuildingManager.Instance.ProcessWorkerBuildingActive && _buildingData.IsThisBuilt)
                {
                    // Количество рабочих в этом здании увеливается
                    GetComponent<ThisBuildingWorkersControl>()
                        .CurrentNumberWorkersInThisBuilding += 1;
                    
                    // GetComponent<ThisBuildingWorkersControl>()
                    //     .NumberOfActiveWorkersInThisBuilding -= 1;
                    
                    // Общее количество рабочих в зданиях увеличивается, количество занятых рабочих уменьшается
                    WorkersInterBuildingControl.Instance.CurrentValueOfWorkers += 1;
                    WorkersInterBuildingControl.Instance.NumberOfActiveWorkers -= 1;

                    // Рабочий уничтожается
                    Destroy(other.gameObject);

                    BuildingManager.Instance.ProcessWorkerBuildingActive = false;
                    return;
                }
                
                // Если данное здание не построено, и прибежавший рабочий занят постройкой
                if (!_buildingData.IsThisBuilt && other.gameObject.GetComponent<WorkerMovementController>().ReadyForWork)
                {
                    // у рабочего пропадает цель следования
                    WorkerMovementController movementController = other.gameObject.GetComponent<WorkerMovementController>();
                    movementController.WorkerPointOfDestination = null;
                    
                    other.transform.LookAt(WorkersInterBuildingControl.CurrentBuilding.transform);
                    
                    // Установка анимаций
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
                    if (thisBuildingWorkersControl.CurrentNumberWorkersInThisBuilding < thisBuildingWorkersControl.MaxValueOfWorkersInThisBuilding)
                    {
                        thisBuildingWorkersControl.CurrentNumberWorkersInThisBuilding += 1;
                        if (GetComponent<EnergyProduction>())
                        {
                            EnergyProduction energyProduction = GetComponent<EnergyProduction>();
                            energyProduction.OnAddEnergy();
                            text.text = $"{_buildingData.Title} запущена ({thisBuildingWorkersControl.CurrentNumberWorkersInThisBuilding}/1) \n Нажмите E чтобы выгрузить рабочего";
                        }
                        else
                        {
                            text.text = $"Нажмите E чтобы выгрузить одного рабочего ({thisBuildingWorkersControl.CurrentNumberWorkersInThisBuilding}/2)";
                        }

                        Destroy(other.gameObject.transform.parent.gameObject);
                        
                        PlayerSaveData playerSaveData = UIManagerLocation.Instance.WhichPlayerDataUse();
                        playerSaveData.BuildingWorkersInformationList[_buildingData.SaveListIndex]
                                .CurrentNumberOfWorkersInThisBuilding =
                            thisBuildingWorkersControl.CurrentNumberWorkersInThisBuilding;
                        return;
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
