using System;
using UnityEngine;
using UnityEngine.AI;
public class WorkerMovementController : MonoBehaviour
{
    [Header("Tutorial")]
    [SerializeField] private TutorialObjective MovementWorkerTutorial;
    [SerializeField] private TutorialObjective WorkerStartMovementToApiaryTutorial;
    private bool IsWorkerMove;
    private bool IsWorkerMovetoApiary;
    
    
    public Transform WorkerPointOfDestination;
    private NavMeshAgent agent;
    public bool ReadyForWork;
    public bool isSelected;
    public bool isSelecting; // Мышь наведена на персонажа
    private Animator anim;
    public GameObject SelectedBuilding;
    [SerializeField] private LayerMask placementLayerMask;
    public Camera MainCamera;
    [SerializeField] private Transform currentWalkingPoint;
    //[SerializeField] private Material OutlineMaterial;
    //[SerializeField] private Color OutlineColor;
    //[SerializeField] private Color BasedOutlineColor;
    [SerializeField] private GameObject OutlineRotate;
    [SerializeField] private GameObject OutlinePOD;
    private Rigidbody _rb;
    void Start()
    {
        ReadyForWork = true;
        agent = GetComponent<NavMeshAgent>();
        isSelected = false;
        isSelecting = false;
        anim = GetComponent<Animator>();
        Debug.Log(agent);
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = new Vector3(0, _rb.linearVelocity.y, 0); // Обнуляем горизонтальную скорость
    }

    void Update()
    {
        if(isSelected){
            if (Input.GetMouseButtonDown(0) && !isSelecting)
            {
                Vector3 point = GetSelectedMapPosition();
                if(SelectedBuilding == null){
                    currentWalkingPoint.transform.position = new Vector3(point.x, point.y, point.z);
                    if (!IsWorkerMove)
                    {
                        MovementWorkerTutorial.CheckAndUpdateTutorialState();
                        IsWorkerMove = true;
                    }
                } else {
                    currentWalkingPoint.transform.position = SelectedBuilding.transform.parent.transform.Find("EndPointWalk").transform.position;
                    if (!IsWorkerMovetoApiary)
                    {
                        WorkerStartMovementToApiaryTutorial.CheckAndUpdateTutorialState();
                        IsWorkerMovetoApiary = true;
                    }
                }
                SetWorkerDestination(currentWalkingPoint.transform, false);
            }
        }

        
        if (WorkerPointOfDestination) 
        {
            
            // Рабочий идет до точки назначения
            anim.SetBool("Idle", false);
            anim.SetBool("Running", true);
            agent.isStopped = false;
            agent.destination = new Vector3(WorkerPointOfDestination.position.x, WorkerPointOfDestination.position.y, WorkerPointOfDestination.position.z);
            
        } 
        else 
        {
            // Рабочий дошел до точки назначения
            agent.isStopped = true;
            anim.SetBool("Running", false);
            anim.SetBool("Idle", true);
            OutlinePOD.SetActive(false);
            if (SelectedBuilding){
                SelectedBuilding = null;
            }
        }
    }

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 lastPosition = Vector3.zero;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = MainCamera.nearClipPlane;
        Ray ray = MainCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10000, placementLayerMask))
        {
            OutlinePOD.SetActive(true);
            lastPosition = hit.point; // SELECTED WALKING POINT
            if(hit.collider.tag == "Building"){
                Debug.Log("Selected building");
                SelectedBuilding = hit.collider.gameObject; // SELECTED BUILDING FOR PHILTWE!!!!!!!!!
                Debug.Log(SelectedBuilding);
            }
        }
        return lastPosition;
    }
    
    
    public void SetWorkerDestination(Transform point, bool isAutomatic){
        if(isAutomatic && SelectedBuilding != null){
            currentWalkingPoint.transform.position = SelectedBuilding.transform.parent.transform.Find("EndPointWalk").transform.position;
            WorkerPointOfDestination = currentWalkingPoint.transform;
            Debug.Log($"Setting destination to: {currentWalkingPoint.transform.position}");
        } else {
            WorkerPointOfDestination = point;
            Debug.Log($"Setting destination to: {point.position}");
        }
    }
    private void OnMouseDown() {
        if (!isSelected) {
            OutlineRotate.SetActive(true);
            isSelected = true;
            //WorkersInterBuildingControl.NumberOfSelectedWorkers += 1;
        } else {
            OutlineRotate.SetActive(false);
            isSelected = false;
            //WorkersInterBuildingControl.NumberOfSelectedWorkers -= 1;
        }
    }

    private void OnMouseEnter() {
        isSelecting = true;
        if(!isSelected)
        {
            OutlineRotate.SetActive(true);
            //OutlineMaterial.color = OutlineColor;
        }
    }

    private void OnMouseExit() {
        isSelecting = false;
        if(!isSelected){
            OutlineRotate.SetActive(false);
            //OutlineMaterial.color = BasedOutlineColor;
        }
    }

    private void OnDisable()
    {
        if (isSelected)
        {
            WorkersInterBuildingControl.NumberOfSelectedWorkers -= 1;
        }
    }
    
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "WalkingPoint"){
            WorkerPointOfDestination = null;
            UpdateWorkerAnimation();
        } 
    }
    
    private void UpdateWorkerAnimation()
    {
        if (WorkerPointOfDestination)
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Running", true);
        }
        else
        {
            anim.SetBool("Running", false);
            anim.SetBool("Idle", true);
            WorkerPointOfDestination = null;
        }
    }
}
