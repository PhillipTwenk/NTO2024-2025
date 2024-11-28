using System;
using UnityEngine;
using UnityEngine.Events;

public class InteractionBuildingController : MonoBehaviour
{
    private bool CanPutE;
    [SerializeField] private bool PossiblityPutEInThisBuilding;
    [SerializeField] private GameEvent OpenDescriptionPanel;
    private BuildingData _buildingData;

    [SerializeField] private UnityEvent InteractionEvent;
    
    public GameEvent OpenBarterMenuEvent;
    public GameEvent CloseBarterMenuEvent;

    public GameObject Texthint;

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
        if (Input.GetButtonDown("InteractionWithBuilding") && CanPutE)
        {
            InteractionEvent?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (PossiblityPutEInThisBuilding)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                CanPutE = true;
                Texthint.SetActive(true);
            }else if(other.gameObject.CompareTag("Worker")){
                if (BuildingManager.Instance.ProcessWorkerBuildingActive)
                {
                    Debug.Log("Рабочий добрался, начинает строить здание");
                    WorkersInterBuildingControl.Instance.NotifyWorkerArrival();
                }
        }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanPutE = false;
            Texthint.SetActive(false);
        }
    }

    /// <summary>
    /// Нажатие на здание
    /// </summary>
    private void OnMouseDown()
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
