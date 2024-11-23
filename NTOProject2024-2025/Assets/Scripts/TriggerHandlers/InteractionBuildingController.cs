using System;
using UnityEngine;

public class InteractionBuildingController : MonoBehaviour
{
    private bool CanPutE;
    [SerializeField] private GameEvent OpenDescriptionPanel;
    private BuildingControl _buildingControl;

    private void Start()
    {
        _buildingControl = GetComponent<BuildingControl>();
        CanPutE = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanPutE = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanPutE = false;
        }
    }

    /// <summary>
    /// Нажатие на здание
    /// </summary>
    private void OnMouseDown()
    {
        AddTextToDescriptionPanel.buildingControl = _buildingControl;
        AddTextToDescriptionPanel.buildingTransform = gameObject.transform;
        
        OpenDescriptionPanel.TriggerEvent();
    }
}
