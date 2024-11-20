using System;
using TMPro;
using UnityEngine;

public class EntryLocationControl : MonoBehaviour
{
    [SerializeField] private GameEvent UpdateResourcesEvent;
    public void InitizilizePLayer()
    {
        // EntityID ActivePlayer = UIManagerLocation.WhichPlayerCreate;
        //
        // if (ActivePlayer.index == 1)
        // {
        //     
        // }else if (ActivePlayer.index == 2)
        // {
        //     
        // }else if (ActivePlayer.index == 3)
        // {
        //     
        // }
        
        UpdateResourcesEvent.TriggerEvent();
    }
}
