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

    [SerializeField] private string NameOfTTS;
    public Transform WorkerPointOfDestination;
    private NavMeshAgent agent;
    public bool ReadyForWork;
    public bool ArriveForBuildBuidling;
    public bool isSelected;
    public bool isSelecting; // Мышь наведена на персонажа
    private Animator anim;
    public GameObject SelectedBuilding; // techTriggerScripts
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
        currentWalkingPoint.gameObject.SetActive(false);
        ReadyForWork = true;
        agent = GetComponent<NavMeshAgent>();
        isSelected = false;
        isSelecting = false;
        anim = GetComponent<Animator>();
        Debug.Log(agent);
        _rb = GetComponent<Rigidbody>(); 
        OutlinePOD.SetActive(false);
        OutlineRotate.SetActive(false);
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
                currentWalkingPoint.gameObject.SetActive(true);
                
                // Если клинкули не на здание
                if(SelectedBuilding == null){
                    currentWalkingPoint.transform.position = new Vector3(point.x, point.y, point.z);
                    ArriveForBuildBuidling = false;
                    if (!IsWorkerMove)
                    {
                        MovementWorkerTutorial.CheckAndUpdateTutorialState();
                        IsWorkerMove = true;
                        Debug.Log("Рабочий начал движение");
                    }
                } else {
                    // Если выбранное здание в процессе строительства и рабочий свободен, он идет его строить
                    if (!SelectedBuilding.gameObject.GetComponent<BuildingData>().IsThisBuilt)
                    {
                        if (ReadyForWork)
                        {
                            ArriveForBuildBuidling = true;
                        }
                    }
                    else
                    {
                        // Если выбранное здание уже построено, проверяем есть ли у него возможность содержать рабочих
                        // Если нет, рабочий не двинется
                        if (!SelectedBuilding.GetComponent<ThisBuildingWorkersControl>())
                        {
                            return;
                        }
                    }
                    
                    
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
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition); 
        RaycastHit hit;
    
        if (Physics.Raycast(ray, out hit, 10000f, placementLayerMask))
        {
            lastPosition = hit.point; // Выбранная точка

            if (hit.collider.CompareTag("Building"))
            {
                if (hit.collider.gameObject.name == NameOfTTS)
                {
                    Debug.Log("Выбрано здание");
                    SelectedBuilding = hit.collider.gameObject; // Выбранное здание
                    Debug.Log(SelectedBuilding);   
                }
            }
            else
            {
                OutlinePOD.SetActive(true);
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
            WorkersInterBuildingControl.SelectedWorker = this.gameObject;
        } else {
            OutlineRotate.SetActive(false);
            isSelected = false;
            WorkersInterBuildingControl.SelectedWorker = this.gameObject;
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
            WorkersInterBuildingControl.SelectedWorker = null;
        }
    }
    
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "WalkingPoint"){
            Debug.Log("Рабочий дошел до точки назначения");
            currentWalkingPoint.gameObject.SetActive(false);
            WorkerPointOfDestination = null;
            anim.SetBool("Running", false);
            anim.SetBool("Idle", true);
        } 
    }
}
