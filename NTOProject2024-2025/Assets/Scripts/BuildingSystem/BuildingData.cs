using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingData : MonoBehaviour
{
    public Building buildingTypeSO;
    public string Title;
    public TextMeshPro textHintBuilding;

    public int Level;
    public int Durability;
    public List<int> Storage;
    public List<int> Production;
    public int HoneyConsumption;
    public int SaveListIndex;
    
    public TextMeshPro AwaitBuildingThisTMPro;

    [TextArea] public string TextAwaitArriveWorker;
    [TextArea] public string TextAwaitBuildingThis;

    public string AwaitWorkerActionText;
    public string AwaitBuildingActionText;

    private async void Start()
    {
        if (Title == "Мобильная база")
        {
            string playerName = UIManagerLocation.WhichPlayerCreate.Name;
            PlayerResources playerResources = await APIManager.Instance.GetPlayerResources(playerName);
            Storage[0] = playerResources.Iron;
            Storage[1] = playerResources.Food;
            Storage[2] = playerResources.CryoCrystal; 
        }
    }

    public void TextPanelBuildingControl(bool IsOpen, string WhichAction)
    {
        if (IsOpen)
        {
            AwaitBuildingThisTMPro.gameObject.SetActive(IsOpen);

            if (WhichAction == AwaitWorkerActionText)
            {
                AwaitBuildingThisTMPro.text = TextAwaitArriveWorker;
            }else if (WhichAction == AwaitBuildingActionText)
            {
                AwaitBuildingThisTMPro.text = TextAwaitBuildingThis;
            }
        }
        else
        {
            AwaitBuildingThisTMPro.gameObject.SetActive(IsOpen);
        }
    }
}
