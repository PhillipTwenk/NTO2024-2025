using UnityEngine;

public class EnemySeekerSystem : MonoBehaviour
{
    private EnemyMovementController moveScript;
    private string[] priorities = {"MainBuilding", "Building", "Worker", "Player"};

    void Start(){
        moveScript = gameObject.transform.parent.GetComponent<EnemyMovementController>();
    }

    private void OnTriggerEnter(Collider other) {
        if (moveScript.SelectedBuilding) {
            return;
        }
        
        foreach (var priority in priorities){
            if(other.CompareTag(priority)){
                if(priority == "MainBuilding" || priority == "Building"){
                    moveScript.SelectedBuilding = other.transform.parent.gameObject;
                    moveScript.SetEnemyDestination(other.transform.parent.transform.Find("EndPointWalk").transform);
                    break;
                } else if (priority == "Worker" || priority == "Player"){
                    moveScript.SelectedBuilding = null;
                    moveScript.SetEnemyDestination(other.transform);
                    break;
                }
            }
        }
    }
}