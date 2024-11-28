using UnityEngine;
using System.Collections.Generic;
using System;
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

    public List<ThisBuildingWorkersControl> listOfActiveBuildingWithWorkers;

    // public GameObject TextHintGameObject;
    // public TextMeshProUGUI HintTextTMPro;

    public event Action IsWorkerHereEvent; // Игрок прибыл

    private bool IsWorkersHere;


    private void Awake()
    {
        Instance = this;
    }

    public void AddNewBuilding(ThisBuildingWorkersControl newBuilding)
    {
        listOfActiveBuildingWithWorkers.Add(newBuilding);
        MaxValueOfWorkers += newBuilding.MaxValueOfWorkersInThisBuilding;
        CurrentValueOfWorkers += newBuilding.CurrentNumberWorkersInThisBuilding;
    }

    public void RemoveNewBuilding(ThisBuildingWorkersControl newBuilding)
    {
        listOfActiveBuildingWithWorkers.Remove(newBuilding);
        MaxValueOfWorkers -= newBuilding.MaxValueOfWorkersInThisBuilding;
        CurrentValueOfWorkers -= newBuilding.CurrentNumberWorkersInThisBuilding;
    }

    ///<summary> 
    /// Отправляет рабочих на строительство / возвращает их обратно
    ///</summary>
    public async Task SendWorkerToBuilding(bool IsSend, BuildingData buildingData)
    {
        if(NumberOfActiveWorkers < CurrentValueOfWorkers && IsSend) // Ержана дернули с кровати и отправили строить крымский мост
        {
            Debug.Log("Рабочий отправился строить здание, ожидаем его прибытия");
            NumberOfActiveWorkers += 1;
            CurrentValueOfWorkers -= 1;
            
            buildingData.TextPanelBuildingControl(true, buildingData.AwaitWorkerActionText);
            
            //Ожидаем прибытия рабочего
            await WaitForWorkerArrival();
            
        }else if(!IsSend) // Отправка рабочего обратно на базу
        {
            NumberOfActiveWorkers -= 1;
            CurrentValueOfWorkers += 1;
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
    public async Task WorkerEndWork(BuildingData buildingData)
    {
        buildingData.TextPanelBuildingControl(true, buildingData.AwaitBuildingActionText);

        await AwaitEndWorking(buildingData);

        Debug.Log("Рабочий достроил, идет обратно");
        
        //Отправляем рабочего обратно на базу
        SendWorkerToBuilding(false, buildingData);

        buildingData.TextPanelBuildingControl(false, buildingData.AwaitBuildingActionText);
    }

    private async Task AwaitEndWorking(BuildingData buildingData)
    {
        var taskCompletionSource = new TaskCompletionSource<bool>();

        Utility.Invoke(this, () => taskCompletionSource.SetResult(true), buildingData.buildingTypeSO.TimeAwaitBuildingThis);

        await taskCompletionSource.Task;
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
