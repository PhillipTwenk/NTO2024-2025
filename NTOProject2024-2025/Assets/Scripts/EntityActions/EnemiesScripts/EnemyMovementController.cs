using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyMovementController : MonoBehaviour
{
    public Transform EnemyPointOfDestination;
    public Transform MainBuildingPosition;
    public GameObject SelectedBuilding;
    private NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        MainBuildingPosition = GameObject.FindWithTag("MainBuilding").transform;
        EnemyPointOfDestination = MainBuildingPosition;
        Debug.Log(agent);
    }

    void Update()
    {
        if (EnemyPointOfDestination) {
            agent.destination = new Vector3(EnemyPointOfDestination.position.x, EnemyPointOfDestination.position.y, EnemyPointOfDestination.position.z);
        } else {
            agent.destination = new Vector3(MainBuildingPosition.Find("EndPointWalk").transform.position.x, MainBuildingPosition.Find("EndPointWalk").transform.position.y, MainBuildingPosition.Find("EndPointWalk").transform.position.z);
        }
    }

    public void SetEnemyDestination(Transform point){
        EnemyPointOfDestination = point;
    }

    public void ResetEnemyDestination(){
        EnemyPointOfDestination = null;
    }
}
