using UnityEngine;
using UnityEngine.AI;
public class WorkerMovementController : MonoBehaviour
{
    public Transform WorkerPointOfDestination;
    private NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Debug.Log(agent);
    }

    void Update()
    {
        if (WorkerPointOfDestination) {
            agent.destination = new Vector3(WorkerPointOfDestination.position.x, WorkerPointOfDestination.position.y, WorkerPointOfDestination.position.z);
        } else {

        }
    }

    public void SetWorkerDestination(Transform point){
        WorkerPointOfDestination = point;
    }

    public void ResetWorkerDestination(){
        WorkerPointOfDestination = null;
    }
}
