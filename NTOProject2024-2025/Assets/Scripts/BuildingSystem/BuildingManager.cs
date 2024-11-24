using System.Collections;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    
    public static BuildingManager Instance { get; private set; }
    
    private bool IsBuildingActive;
    public bool CanBuilding;
    
    [SerializeField] private Camera MainCamera;
    private Vector3 lastPosition;
    
    public GameObject MouseIndicator;
    [SerializeField] private LayerMask placementLayerMask;

    [SerializeField] private float YplaceVector;

    public GameObject CurrentBuilding;

    [SerializeField] private float awaitValueBuild;

    public void StartPlacingBuild() => IsBuildingActive = true;
    public void EndPlacingBuild() => IsBuildingActive = false;

    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        IsBuildingActive = false;
    }

    private void Update()
    {
        if (IsBuildingActive)
        {
            Vector3 mousePosition = GetSelectedMapPosition();
            MouseIndicator.transform.position = new Vector3(mousePosition.x, mousePosition.y + 0.5f, mousePosition.z);
            
            if (Input.GetMouseButtonDown(0) && CanBuilding)
            {
                PlaceBuilding(mousePosition);
            }
        }
    }

    /// <summary>
    /// Возвращает позицию мыши
    /// </summary>
    /// <returns></returns>
    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = MainCamera.nearClipPlane;
        Ray ray = MainCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayerMask))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }
    
    /// <summary>
    /// Размещение строения
    /// </summary>
    /// <param name="mousePosition"></param>
    public void PlaceBuilding(Vector3 mousePosition)
    {
        Debug.Log(mousePosition);
        MouseIndicator.transform.position = new Vector3(mousePosition.x, YplaceVector, mousePosition.z); 
        GameObject newBuildingObject = Instantiate(CurrentBuilding);
        newBuildingObject.transform.position = MouseIndicator.transform.position;
        Debug.Log(newBuildingObject.transform.position);
        Destroy(MouseIndicator);
        CurrentBuilding = null;
        IsBuildingActive = false;
        CanBuilding = true;
        //StartCoroutine(TimerBuildingCoroutine(awaitValueBuild));
    }

    // private IEnumerator TimerBuildingCoroutine(float await)
    // {
    //     
    // } 
}
