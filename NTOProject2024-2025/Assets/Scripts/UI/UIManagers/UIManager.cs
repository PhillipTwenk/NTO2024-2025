using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameEvent OpenBuildingPanelEvent;
    public GameEvent CloseBuildingPanelEvent;

    [SerializeField] private Transform NewPlanPosition;
    [SerializeField] private Transform ContentPanel;

    private bool IsOpenBuildingPanel;

    private void Start()
    {
        IsOpenBuildingPanel = true;
    }

    private void Update()
    {
        if (Input.GetButtonDown("OpenBuildingPanel") && IsOpenBuildingPanel)
        {
            Debug.Log("Открыта панель строительства");
            OpenBuildingPanelEvent.TriggerEvent();
            IsOpenBuildingPanel = false;
            return;
        }
        if (Input.GetButtonDown("OpenBuildingPanel") && !IsOpenBuildingPanel)
        {
            Debug.Log("Закрыта панель строительства");
            CloseBuildingPanelEvent.TriggerEvent();
            IsOpenBuildingPanel = true;
            return;
        }
    }

    /// <summary>
    /// Добавляет возможность строить новое здание после покупки нового чертежа
    /// </summary>
    public void AddNewPlanInPanel(Plan plan)
    {
        GameObject newPlanGameObject = Instantiate(plan.PlanPrefab, NewPlanPosition);

        newPlanGameObject.transform.SetParent(ContentPanel);
        
        TextMeshProUGUI titleTMPro = newPlanGameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI descriptionTMPro = newPlanGameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        Image sprite = newPlanGameObject.transform.GetChild(2).GetComponent<Image>();
        TextMeshProUGUI durabilityTMPro = newPlanGameObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI energyHoneyConsumptionTMPro = newPlanGameObject.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI resourceProductionTMPro = newPlanGameObject.transform.GetChild(5).GetComponent<TextMeshProUGUI>();

        titleTMPro.text = plan.Title;
        descriptionTMPro.text = plan.Description;
        sprite.sprite = plan.planSprite;
        durabilityTMPro.text = $"- Прочность: {plan.durability}";
        energyHoneyConsumptionTMPro.text = $"- Потребляет: {plan.energyHoneyConsumption}";
        resourceProductionTMPro.text = $"- Производит: {plan.resourceProduction}";
    }
}
