using System;
using System.Collections;
using System.Collections.Generic;
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
    public bool isSelecting;
    private Animator anim;
    public GameObject SelectedBuilding;
    [SerializeField] private Outline outlineMode;
    [SerializeField] private LayerMask placementLayerMask;
    public Camera MainCamera;
    [SerializeField] public LineRenderer line;
    [SerializeField] private Transform currentWalkingPoint;
    void Start()
    {
        ReadyForWork = true;
        agent = GetComponent<NavMeshAgent>();
        outlineMode = GetComponent<Outline>();
        outlineMode.enabled = false;
        isSelected = false;
        isSelecting = false;
        anim = GetComponent<Animator>();
        Debug.Log(agent);
    }

    void Update()
    {
        // if(WorkersInterBuildingControl.SelectedWorker != gameObject && WorkersInterBuildingControl.SelectedWorker != null){
        //     isSelected = false;
        // }

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

        if (WorkerPointOfDestination) {
            //Debug.Log($"Moving to: {WorkerPointOfDestination.position}");
            anim.SetBool("Idle", false);
            anim.SetBool("Running", true);
            agent.destination = new Vector3(WorkerPointOfDestination.position.x, WorkerPointOfDestination.position.y, WorkerPointOfDestination.position.z);
            if (agent.path.status == NavMeshPathStatus.PathComplete) {
                line.enabled = true;
                line.SetPosition(0, transform.position);
                DrawPath(agent.path);
            }
        } else {
            anim.SetBool("Running", false);
            anim.SetBool("Idle", true);
            if (SelectedBuilding){
                SelectedBuilding = null;
            }
        }
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

    public void ResetWorkerDestination(){
        WorkerPointOfDestination = null;
    }

    private void OnMouseDown() {
        if (!isSelected) {
            outlineMode.enabled = true;
            outlineMode.OutlineWidth = 5f;
            isSelected = true;
            WorkersInterBuildingControl.NumberOfSelectedWorkers += 1;
        } else {
            outlineMode.enabled = false;
            isSelected = false;
            WorkersInterBuildingControl.NumberOfSelectedWorkers -= 1;
        }
    }

    private void OnMouseEnter() {
        isSelecting = true;
        if(!isSelected){
            outlineMode.enabled = true;
            outlineMode.OutlineWidth = 2f;
        }
    }

    private void OnMouseExit() {
        isSelecting = false;
        if(!isSelected){
            outlineMode.enabled = false;
        }
    }

    private void OnDisable()
    {
        if (isSelected)
        {
            WorkersInterBuildingControl.NumberOfSelectedWorkers -= 1;
        }
    }

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 lastPosition = Vector3.zero;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = MainCamera.nearClipPlane;
        Ray ray = MainCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2000, placementLayerMask))
        {
            lastPosition = hit.point; // SELECTED WALKING POINT
            if(hit.collider.tag == "Building"){
                Debug.Log("Selected building");
                SelectedBuilding = hit.collider.gameObject; // SELECTED BUILDING FOR PHILTWE!!!!!!!!!
                Debug.Log(SelectedBuilding);
            }
        }
        return lastPosition;
    }

    public void DrawPath(NavMeshPath path){
        if(path.corners.Length < 2) return;

        line.SetVertexCount(path.corners.Length); 
        for(var i = 1; i < path.corners.Length; i++){
            line.SetPosition(i, path.corners[i]); 
        }
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log("1");
        if(other.collider.tag == "WalkingPoint"){
            Debug.Log("2");
            line.enabled = false;
            WorkerPointOfDestination = null;
            anim.SetBool("Running", false);
            anim.SetBool("Idle", true);
        } else if (other.collider.tag == "Building"){
            // PHILTWE IS WORKING HERE!!!!!!!!!!!!!!!!!!!!
        }
    }
}
