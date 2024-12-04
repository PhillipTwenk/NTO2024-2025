using System;
using System.Collections.Generic;
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
        playerResources.Energy += honeyProduction;
        playerResources.Food += foodProduction;

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
        playerResources.Energy -= honeyProduction;
        playerResources.Food -= foodProduction;

        await APIManager.Instance.PutPlayerResources(playerName, playerResources.Iron, playerResources.Energy,
            playerResources.Food, playerResources.CryoCrystal);
        ResourceUpdateEvent.TriggerEvent();
        LoadingCanvasController.Instance.LoadingCanvasTransparent.SetActive(false);
    }
}
