using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ThisBuildingWorkersControl : MonoBehaviour
{
    public int CurrentNumberWorkersInThisBuilding;
    public int MaxValueOfWorkersInThisBuilding;
    public int NumberOfActiveWorkersInThisBuilding;

    public Transform buildingSpawnWorkerPointTransform;

    public GameObject WorkerPrefab;

    public void AddWorkersInThisBuilding(int number, bool IsAdd)
    {
        if (IsAdd)
        {
            if (!(CurrentNumberWorkersInThisBuilding <= MaxValueOfWorkersInThisBuilding))
            {
                CurrentNumberWorkersInThisBuilding += number;
            }
        }else if(!IsAdd){
            if (!(CurrentNumberWorkersInThisBuilding > 0))
            {
                CurrentNumberWorkersInThisBuilding -= number;
            }
        }
    }

    public void StartMovementWorkerToBuilding(bool End, Transform buildingTransform, WorkerMovementController movementController, Animator animator)
    {
        if (!End)
        {
            Transform workerTransform = movementController.transform;
            List<Transform> pointsOfBuildings = buildingTransform.gameObject.transform.GetChild(0).GetComponent<InteractionBuildingController>()
                .PointsOfBuildings;
        
            Transform pointForBuild = null;
            float distanceToPoint = 0;
            int i = 0;
            foreach (var point in pointsOfBuildings)
            {
                if (i == 0)
                {
                    pointForBuild = point;
                    distanceToPoint = Vector3.Distance(workerTransform.position, point.position);
                    i++;
                    continue;
                }
                if (Vector3.Distance(workerTransform.position, point.position) < distanceToPoint)
                {
                    pointForBuild = point;
                    distanceToPoint = Vector3.Distance(workerTransform.position, point.position);
                    i++;
                }
            }
            
            movementController.gameObject.GetComponent<NavMeshAgent>().CompleteOffMeshLink();
            movementController.SetWorkerDestination(pointForBuild);
        }
        else
        {
            movementController.gameObject.GetComponent<NavMeshAgent>().CompleteOffMeshLink();
            movementController.SetWorkerDestination(buildingTransform);
        }

        animator.SetBool("Running", true);
        animator.SetBool("Building", false);
        animator.SetBool("Idle", false);
    }

}
