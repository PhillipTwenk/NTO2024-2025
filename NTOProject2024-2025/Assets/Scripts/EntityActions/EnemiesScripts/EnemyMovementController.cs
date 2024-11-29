using UnityEngine;
using UnityEngine.AI;
public class EnemyMovementController : MonoBehaviour
{
    public Transform EnemyPointOfDestination;
    public Transform MainBuildingPosition;
    private NavMeshAgent Agent;
    private string[] priorityList = {"MainBuilding", "Building", "Worker", "Player"}; // Лист приоритетов для врага 
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Debug.Log(Agent);
    }

    void Update()
    {
        if (EnemyPointOfDestination) {
            Agent.destination = new Vector3(EnemyPointOfDestination.position.x, EnemyPointOfDestination.position.y, EnemyPointOfDestination.position.z);
        } else {
            Agent.destination = new Vector3(MainBuildingPosition.position.x, MainBuildingPosition.position.y, MainBuildingPosition.position.z);
        }
    }

    public void SetEnemyDestination(Transform point){
        EnemyPointOfDestination = point;
    }

    public void ResetEnemyDestination(){
        EnemyPointOfDestination = null;
    }

    private void OnCollisionEnter(Collision other) {
        foreach (var priority in priorityList){
            if (priority == other.gameObject.tag){
                EnemyPointOfDestination = other.transform;
            }
        }
    }
}
