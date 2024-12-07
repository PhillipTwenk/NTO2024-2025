using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;


public class ThisBuildingWorkersControl : MonoBehaviour
{
    [Header("Tutorial")]
    [SerializeField] private TutorialObjective CreateNewWorkerTutorial;
    
    public int CurrentNumberWorkersInThisBuilding;
    public int MaxValueOfWorkersInThisBuilding;
    public int NumberOfActiveWorkersInThisBuilding;

    public Transform buildingSpawnWorkerPointTransform;

    public GameObject WorkerPrefab;
    public Camera MainCamera;

    public void SpawnWorkersInThisBuilding(TextMeshPro text)
    {
        if (CurrentNumberWorkersInThisBuilding > 0)
        {
            CurrentNumberWorkersInThisBuilding -= 1;
            GameObject newWorker = Instantiate(WorkerPrefab, null);
            newWorker.transform.position = buildingSpawnWorkerPointTransform.position;
            newWorker.transform.rotation = buildingSpawnWorkerPointTransform.rotation;
            newWorker.transform.SetParent(null);
            newWorker.transform.GetChild(0).GetComponent<WorkerMovementController>().MainCamera = WorkersInterBuildingControl.MainCamera;
            text.text = $"Нажмите E чтобы выгрузить одного рабочего ({CurrentNumberWorkersInThisBuilding}/2)";
            CreateNewWorkerTutorial.CheckAndUpdateTutorialState();
        }
    }

    public void TextChanger(TextMeshPro text)
    {
        text.text = $"Нажмите E чтобы выгрузить одного рабочего ({CurrentNumberWorkersInThisBuilding}/2)";
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
            movementController.SetWorkerDestination(pointForBuild, true);
        }
        else
        {
            movementController.gameObject.GetComponent<NavMeshAgent>().CompleteOffMeshLink();
            movementController.SetWorkerDestination(buildingTransform, true);
        }

        animator.SetBool("Running", true);
        animator.SetBool("Building", false);
        animator.SetBool("Idle", false);
    }

}
