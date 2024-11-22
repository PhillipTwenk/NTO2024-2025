using System;
using UnityEngine;

public class YouCantBuildHereTrigger : MonoBehaviour
{
    public Material material;
    private void OnTriggerStay(Collider other)
    {
        material.color = Color.red;
        BuildingManager.Instance.CanBuilding = false;
    }

    private void OnTriggerExit(Collider other)
    {
        material.color = Color.green;
        BuildingManager.Instance.CanBuilding = true;
    }
}
