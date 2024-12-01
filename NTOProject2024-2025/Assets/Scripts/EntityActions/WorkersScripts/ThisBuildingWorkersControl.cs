using System;
using UnityEngine;



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

    public void StartMovementWorkerToBuilding(Transform buildingTransform, WorkerMovementController movementController, Animator animator)
    {
        movementController.SetWorkerDestination(buildingTransform);
        
        //Animator action
    }

    public void WorkerArrive()
    {
        
    }

    
}
