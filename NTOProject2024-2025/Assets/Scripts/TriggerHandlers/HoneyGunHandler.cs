using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class HoneyGunHandler : MonoBehaviour
{
    public Transform CannonTower;
    public Transform PointOfShoot;
    public Bullet bulletSO;
    private BuildingData _buildingData;

    private void Start()
    {
        _buildingData = GetComponent<BuildingData>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(""))
        {
            CannonTower.LookAt(other.transform);
            StartCoroutine(ShootCycle());
        }
    }

    private IEnumerator ShootCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(_buildingData.Production[0]);
        }
    }
}
