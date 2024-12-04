using UnityEngine;
using UnityEngine.AI;
public class WorkerMovementController : MonoBehaviour
{
    public Transform WorkerPointOfDestination;
    private NavMeshAgent agent;
    public bool ReadyForWork;
    public bool isSelected;
    [SerializeField] private Outline outlineMode;
    void Start()
    {
        ReadyForWork = true;
        agent = GetComponent<NavMeshAgent>();
        outlineMode = GetComponent<Outline>();
        outlineMode.enabled = false;
        isSelected = false;
        Debug.Log(agent);
    }

    void Update()
    {
        if (WorkerPointOfDestination) {
            //Debug.Log($"Moving to: {WorkerPointOfDestination.position}");
            agent.destination = new Vector3(WorkerPointOfDestination.position.x, WorkerPointOfDestination.position.y, WorkerPointOfDestination.position.z);
        }
    }

    public void SetWorkerDestination(Transform point){
        Debug.Log($"Setting destination to: {point.position}");
        WorkerPointOfDestination = point;
    }

    public void ResetWorkerDestination(){
        WorkerPointOfDestination = null;
    }

    private void OnMouseDown() {
        outlineMode.enabled = true;
        outlineMode.OutlineWidth = 5f;
        isSelected = true;
    }

    private void OnMouseEnter() {
        outlineMode.enabled = true;
        outlineMode.OutlineWidth = 2f;
    }

    private void OnMouseExit() {
        if(!isSelected){
            outlineMode.enabled = false;
        }
    }
}
