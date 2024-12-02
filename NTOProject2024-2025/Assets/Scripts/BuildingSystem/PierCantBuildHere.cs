using UnityEngine;

public class PierCantBuildHere : MonoBehaviour
{
    public Material material;

    private void Start()
    {
        material.color = Color.red;
        BuildingManager.Instance.CanBuilding = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("PierPlace"))
        {
            material.color = Color.green;
            BuildingManager.Instance.CanBuilding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PierPlace"))
        {
            material.color = Color.red;
            BuildingManager.Instance.CanBuilding = false;
        }
    }
}
