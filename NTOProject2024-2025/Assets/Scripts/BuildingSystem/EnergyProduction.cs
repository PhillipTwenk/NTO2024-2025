using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class EnergyProduction : MonoBehaviour
{
    private BuildingData _buildingData;
    [SerializeField] private GameEvent ResourceUpdateEvent;
    [SerializeField] private GameEvent WorkerGoToThisBuilding;

    // private void OnEnable()
    // {
    //     _buildingData = GetComponent<BuildingData>();
    //     if (_buildingData.Title == "Мобильная база")
    //     {
    //         _buildingData.StartBuildingFunctionEvent?.Invoke();
    //     }
    // }

    private void OnMouseDown()
    {
        _buildingData = GetComponent<BuildingData>();
        if (_buildingData.Storage[0] < 1)
        {
            WorkerGoToThisBuilding.TriggerEvent();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Worker") && other.gameObject.GetComponent<WorkerMovementController>().isSelected && other.gameObject.GetComponent<WorkerMovementController>().ReadyForWork)
        {
            _buildingData.Storage[0] += 1;
        }
    }

    public async void OnAddEnergy()
    {
        _buildingData = GetComponent<BuildingData>();
        
        LoadingCanvasController.Instance.LoadingCanvasTransparent.SetActive(true);
        
        int honeyProduction = _buildingData.Production[0];
        int foodProduction = _buildingData.Production[1];
        
        Debug.Log($"Производство меда: {honeyProduction}");

        string playerName = UIManagerLocation.WhichPlayerCreate.Name;
        PlayerResources playerResources =
            await APIManager.Instance.GetPlayerResources(playerName);
        int OldEnergyValue = playerResources.Energy;
        int OldFoodValue = playerResources.Food;
        playerResources.Energy += honeyProduction;
        playerResources.Food += foodProduction;
        LogSender(playerName, "Пасека начала производство энергии и мёда", playerResources.Energy - OldEnergyValue, playerResources.Food - OldFoodValue);

        await APIManager.Instance.PutPlayerResources(playerName, playerResources.Iron, playerResources.Energy,
            playerResources.Food, playerResources.CryoCrystal);
        
        ResourceUpdateEvent.TriggerEvent();
        LoadingCanvasController.Instance.LoadingCanvasTransparent.SetActive(false);
    }

    private async void OnDisable()
    {
        _buildingData = GetComponent<BuildingData>();
        
        LoadingCanvasController.Instance.LoadingCanvasTransparent.SetActive(true);    
        
        int honeyProduction = _buildingData.Production[0];
        int foodProduction = _buildingData.Production[1];

        string playerName = UIManagerLocation.WhichPlayerCreate.Name;
        PlayerResources playerResources =
            await APIManager.Instance.GetPlayerResources(playerName);
        int OldEnergyValue = playerResources.Energy;
        int OldFoodValue = playerResources.Food;
        playerResources.Energy -= honeyProduction;
        playerResources.Food -= foodProduction;
        LogSender(playerName, "Пасека прекратила производство энергии и мёда", playerResources.Energy - OldEnergyValue, playerResources.Food - OldFoodValue );
        await APIManager.Instance.PutPlayerResources(playerName, playerResources.Iron, playerResources.Energy,
            playerResources.Food, playerResources.CryoCrystal);
        ResourceUpdateEvent.TriggerEvent();
        LoadingCanvasController.Instance.LoadingCanvasTransparent.SetActive(false);
    }

    private void LogSender(string playerName, string comment, int ChangeEnergy, int ChangeFood)
    {
        Dictionary<string,string> playerDictionary = new Dictionary<string, string>();
        playerDictionary.Add("EnergyValueUpdate", $"{ChangeEnergy}");
        playerDictionary.Add("FoodValueUpdate", $"{ChangeFood}");
        APIManager.Instance.CreatePlayerLog(comment, playerName, playerDictionary);
    }
}
