using UnityEngine;

public class MinerCantBuildHere : MonoBehaviour
{
    public Material material;

    private void Start()
    {
        material.color = Color.red;
        BuildingManager.Instance.CanBuilding = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("IronMinerPlace") || other.gameObject.CompareTag("CCminerPlace"))
        {
            material.color = Color.green;
            BuildingManager.Instance.CanBuilding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("IronMinerPlace") || other.gameObject.CompareTag("CCminerPlace"))
        {
            material.color = Color.red;
            BuildingManager.Instance.CanBuilding = false;
        }
    }
}
