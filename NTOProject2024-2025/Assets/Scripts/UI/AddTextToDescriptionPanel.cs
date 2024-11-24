using System;
using TMPro;
using UnityEngine;

public class AddTextToDescriptionPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Title;
    [SerializeField] private TextMeshProUGUI Level;
    [SerializeField] private TextMeshProUGUI Durability;
    [SerializeField] private TextMeshProUGUI Production;
    [SerializeField] private TextMeshProUGUI HoneyConsumption;
    [SerializeField] private TextMeshProUGUI Storage;

    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject point;

    public static BuildingControl buildingControl;
    public static Transform buildingTransform;

    [SerializeField] private Transform pointInPanelAngle1;
    [SerializeField] private Transform pointInPanelAngle2;
    [SerializeField] private Transform pointInPanelAngle3;
    [SerializeField] private Transform pointInPanelAngle4;
    
    public LineRenderer lineRenderer;
    public Camera mainCamera;

    private void Start()
    {
        panel.SetActive(false);
        point.SetActive(false);
        lineRenderer.enabled = false;
    }

    /// <summary>
    /// Нажали на здание, открытие панели подробной информации
    /// </summary>
    /// <param name="building"></param>
    public void ShowDescriptionPanel()
    {
        point.SetActive(true);
        panel.SetActive(true);
        
        // Центр экрана
        float centerX = Screen.width / 2;
        float centerY = Screen.height / 2;

        // Экранные координаты объекта
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(buildingTransform.position);
        
        // Определяем четверть
        if (screenPosition.x > centerX && screenPosition.y > centerY)
        {
            Debug.Log("Объект находится в первой четверти.");
            Vector3 panelPosition = screenPosition + new Vector3(-100, -50, 0);
            panel.transform.position = panelPosition;
            
            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, lineRenderer.gameObject.transform.position);
            lineRenderer.SetPosition(1, mainCamera.ScreenToWorldPoint(new Vector3(pointInPanelAngle1.position.x, pointInPanelAngle1.position.x, mainCamera.nearClipPlane)));
        }
        else if (screenPosition.x < centerX && screenPosition.y > centerY)
        {
            Debug.Log("Объект находится во второй четверти.");
            Vector3 panelPosition = screenPosition + new Vector3(100, -50, 0);
            panel.transform.position = panelPosition;
            
            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, lineRenderer.gameObject.transform.position);
            lineRenderer.SetPosition(1, mainCamera.ScreenToWorldPoint(new Vector3(pointInPanelAngle2.position.x, pointInPanelAngle2.position.x, mainCamera.nearClipPlane)));
        }
        else if (screenPosition.x < centerX && screenPosition.y < centerY)
        {
            Debug.Log("Объект находится в третьей четверти.");
            Vector3 panelPosition = screenPosition + new Vector3(100, 50, 0);
            panel.transform.position = panelPosition;
            
            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, lineRenderer.gameObject.transform.position);
            lineRenderer.SetPosition(1, mainCamera.ScreenToWorldPoint(new Vector3(pointInPanelAngle3.position.x, pointInPanelAngle3.position.x, mainCamera.nearClipPlane)));
        }
        else if (screenPosition.x > centerX && screenPosition.y < centerY)
        {
            Debug.Log("Объект находится в четвёртой четверти.");
            Vector3 panelPosition = screenPosition + new Vector3(-100, 50, 0);
            panel.transform.position = panelPosition;
            
            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, lineRenderer.gameObject.transform.position);
            lineRenderer.SetPosition(1, mainCamera.ScreenToWorldPoint(new Vector3(pointInPanelAngle4.position.x, pointInPanelAngle4.position.x, mainCamera.nearClipPlane)));
        }
        
        Title.text = buildingControl.Title;
        Level.text = Convert.ToString(buildingControl.Level);
        Durability.text = Convert.ToString(buildingControl.Durability);
        Production.text = Convert.ToString(buildingControl.Production);
        HoneyConsumption.text = Convert.ToString(buildingControl.HoneyConsumption);
        Storage.text = Convert.ToString(buildingControl.Storage);
    }
}
