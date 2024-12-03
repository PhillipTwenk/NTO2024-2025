using System;
using System.Collections.Generic;
using UnityEngine;

public class EnergyProduction : MonoBehaviour
{
    private BuildingData _buildingData;
    [SerializeField] private GameEvent ResourceUpdateEvent;

    // private void OnEnable()
    // {
    //     _buildingData = GetComponent<BuildingData>();
    //     if (_buildingData.Title == "Мобильная база")
    //     {
    //         _buildingData.StartBuildingFunctionEvent?.Invoke();
    //     }
    // }

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
