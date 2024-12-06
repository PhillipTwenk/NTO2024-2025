using System;
using TMPro;
using UnityEngine;

public class EntryLocationControl : MonoBehaviour
{
    [SerializeField] private GameEvent UpdateResourcesEvent;
    [SerializeField] private GameEvent BaseSongStartEvent;
    public void InitizilizePLayer()
    {
        UpdateResourcesEvent.TriggerEvent();

        PlayerSaveData pLayerSaveData = UIManagerLocation.Instance.WhichPlayerDataUse();
        pLayerSaveData.InitializeBuildings();
        BaseSongStartEvent.TriggerEvent();
    }
}
