using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using TMPro;
using System.Threading.Tasks;

public class WorkersInterBuildingControl : MonoBehaviour
{
    public static WorkersInterBuildingControl Instance { get; set;}

    //[TextArea] [SerializeField] private string HintTextNotEnoughtWorkers;
    //[SerializeField] private float TimeHintActive;
    [TextArea] [SerializeField] private string HintAwaitArriveWorker;
    [TextArea] [SerializeField] private string HintAwaitBuilding;

    public int CurrentValueOfWorkers;
    public int MaxValueOfWorkers;
    public int NumberOfActiveWorkers;

    public static BuildingData CurrentBuilding;

    public List<ThisBuildingWorkersControl> listOfActiveBuildingWithWorkers;

    // public GameObject TextHintGameObject;
    // public TextMeshProUGUI HintTextTMPro;

    public event Action IsWorkerHereEvent; // Игрок прибыл
    public static GameObject SelectedWorker;
    public Camera mainCamera;
    public static Camera MainCamera;

    private bool IsWorkersHere;


    private void Awake()
    {
        Instance = this;
        MainCamera = mainCamera;
        CurrentBuilding = null;
    }

    public void AddNewBuilding(ThisBuildingWorkersControl newBuilding)
    {
        if (newBuilding != null) 
        {
            listOfActiveBuildingWithWorkers.Add(newBuilding);
            MaxValueOfWorkers += newBuilding.MaxValueOfWorkersInThisBuilding;
            CurrentValueOfWorkers += newBuilding.CurrentNumberWorkersInThisBuilding;
        }
        else
        {
            listOfActiveBuildingWithWorkers.Add(newBuilding);
        }
    }

    public void RemoveNewBuilding(ThisBuildingWorkersControl newBuilding)
    {
        if (newBuilding != null)
        {
            listOfActiveBuildingWithWorkers.Remove(newBuilding);
            MaxValueOfWorkers -= newBuilding.MaxValueOfWorkersInThisBuilding;
            CurrentValueOfWorkers -= newBuilding.CurrentNumberWorkersInThisBuilding;
        }
        else
        {
            listOfActiveBuildingWithWorkers.Remove(newBuilding);
        }
    }

    ///<summary> 
    /// Отправляет рабочих на строительство / возвращает их обратно
    ///</summary>
    public async Task SendWorkerToBuilding(bool IsSend, BuildingData buildingData, Transform buildingTransform)
    {
        if(NumberOfActiveWorkers < CurrentValueOfWorkers && IsSend) // Ержана дернули с кровати и отправили строить крымский мост
        {
            CurrentBuilding = buildingData;
            
            Debug.Log("Рабочий отправился строить здание, ожидаем его прибытия");
            NumberOfActiveWorkers += 1;
            CurrentValueOfWorkers -= 1;

            buildingData.TextPanelBuildingControl(true, buildingData.AwaitWorkerActionText);

            SendWorkerToBuildingAnimationControl(buildingTransform);
            
            //Ожидаем прибытия рабочего
            await WaitForWorkerArrival();
            
        }else if(!IsSend) // Отправка рабочего обратно на базу
        {
            NumberOfActiveWorkers -= 1;
            CurrentValueOfWorkers += 1;
            CurrentBuilding = null;
        }else
        {
            //ShowHint(HintTextNotEnoughtWorkers);
            Debug.Log("Нет рабочих");
        }
    }

    ///<summary> 
    /// Ожидание прибытия рабочего
    ///</summary>
    private async Task WaitForWorkerArrival()
    {
        // Создаем задачу, которая завершится при вызове события
        var taskCompletionSource = new TaskCompletionSource<bool>();

        void OnWorkerHere()
        {
            IsWorkerHereEvent -= OnWorkerHere;
            taskCompletionSource.SetResult(true);
        }

        IsWorkerHereEvent += OnWorkerHere;

        // Ждем завершения задачи
        await taskCompletionSource.Task;
    }

    ///<summary> 
    /// Вызывается из триггера здания, когда рабочий добежал до здания
    ///</summary>
    public void NotifyWorkerArrival()
    {
        IsWorkersHere = true;
        IsWorkerHereEvent?.Invoke();
    }


    ///<summary> 
    /// Ожидание завершения строительства
    ///</summary>
    public async Task WorkerEndWork(BuildingData buildingData, Transform buildingTransform)
    {
        buildingData.TextPanelBuildingControl(true, buildingData.AwaitBuildingActionText);

        await AwaitEndWorking(buildingData);

        //Debug.Log("Рабочий достроил, идет обратно");
        
        buildingData.TextPanelBuildingControl(false, buildingData.AwaitBuildingActionText);
    }

    private async Task AwaitEndWorking(BuildingData buildingData)
    {
        var taskCompletionSource = new TaskCompletionSource<bool>();

        Utility.Invoke(this, () => taskCompletionSource.SetResult(true), buildingData.buildingTypeSO.TimeAwaitBuildingThis);

        await taskCompletionSource.Task;
    }


    /// <summary>
    /// Находит свободного рабочего к постройке здания
    /// </summary>
    public void SendWorkerToBuildingAnimationControl(Transform building)
    {
        foreach (var buildingControl in listOfActiveBuildingWithWorkers)
        {
            if (buildingControl != null)
            {
                if (buildingControl.CurrentNumberWorkersInThisBuilding > 0)
                {
                    buildingControl.NumberOfActiveWorkersInThisBuilding += 1;
                    buildingControl.CurrentNumberWorkersInThisBuilding -= 1;
                    
                    Transform buildingSpawnWorkerPointTransform = buildingControl.buildingSpawnWorkerPointTransform;

                    GameObject newWorker = Instantiate(buildingControl.WorkerPrefab);
                    newWorker.transform.position = buildingSpawnWorkerPointTransform.position;
               
                    WorkerMovementController workerMovementController =
                        newWorker.GetComponent<WorkerMovementController>();
                    Animator animator = newWorker.GetComponent<Animator>();
                    buildingControl.StartMovementWorkerToBuilding(false, building, workerMovementController, animator);

                    return;
                }
            }
        }
    }

    /// <summary>
    /// Начинает анимацию строительства
    /// </summary>
    public async void StartAnimationBuilding(Animator animator, WorkerMovementController movementController, BuildingData buildingData)
    {
        Debug.Log(123);
        
        await AwaitEndWorking(buildingData);
        
        buildingData.StartBuildingFunctionEvent?.Invoke();
        
        Debug.Log(456);
        
        movementController.ReadyForWork = false;

        EndWorkingAnimationControl(movementController, animator);
    }

    public void EndWorkingAnimationControl(WorkerMovementController movementController, Animator animator)
    {
        foreach (var buildingControl in listOfActiveBuildingWithWorkers)
        {
            if (buildingControl != null)
            {
                if (buildingControl.CurrentNumberWorkersInThisBuilding < buildingControl.MaxValueOfWorkersInThisBuilding)
                {
                    Debug.Log(789);

                    CurrentBuilding = buildingControl.gameObject.GetComponent<BuildingData>();
                    Debug.Log(CurrentBuilding.Title);
                    Transform buildingSpawnWorkerPointTransform = buildingControl.buildingSpawnWorkerPointTransform;
                    
                    buildingControl.StartMovementWorkerToBuilding(true, buildingSpawnWorkerPointTransform, movementController, animator);

                    return;
                }
            }
        }
    }
    
    ///<summary> 
    /// Открытие / закрытие панели с подсказкой
    ///</summary>
    // private void ShowHint(string message)
    // {
    //     TextHintGameObject.SetActive(true);
    //     Utility.Invoke(this, () => TextHintGameObject.SetActive(false), TimeHintActive);
    //     HintTextTMPro.text = message;
    // }
}
