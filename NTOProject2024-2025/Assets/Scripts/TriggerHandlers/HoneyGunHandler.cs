using System.Collections;
using UnityEngine;

public class HoneyGunHandler : MonoBehaviour
{
    public Transform CannonTower; // Башня, которая поворачивается
    public Transform PointOfShoot; // Точка, из которой вылетают снаряды
    public Bullet bulletSO; // ScriptableObject с данными снаряда
    public BuildingData _buildingData; // Данные здания (скорость стрельбы)
    public float rotationSpeed; // Скорость поворота башни

    private Transform currentTarget; // Текущая цель
    private Coroutine shootingCoroutine; // Ссылка на корутину стрельбы

    private void Update()
    {
        // Если есть цель
        if (currentTarget != null)
        {
            // Поворот башни к цели (только по горизонтали)
            Vector3 direction = (new Vector3(currentTarget.position.x, CannonTower.position.y, currentTarget.position.z) - CannonTower.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            CannonTower.rotation = Quaternion.Lerp(CannonTower.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            // Если корутина стрельбы еще не запущена
            if (shootingCoroutine == null)
            {
                shootingCoroutine = StartCoroutine(ShootCycle());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Установка текущей цели
            currentTarget = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && other.transform == currentTarget)
        {
            // Сброс цели при выходе врага из зоны триггера
            currentTarget = null;

            // Остановка корутины стрельбы
            if (shootingCoroutine != null)
            {
                StopCoroutine(shootingCoroutine);
                shootingCoroutine = null;
            }
        }
    }

    private IEnumerator ShootCycle()
    {
        while (currentTarget != null)
        {
            // Создание снаряда
            GameObject newBullet = Instantiate(bulletSO.BulletPrefab, PointOfShoot.position, PointOfShoot.rotation);

            // Установка скорости снаряда
            Rigidbody rb = newBullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = PointOfShoot.forward * bulletSO.Speed;
            }
            else
            {
                Debug.LogError("У снаряда отсутствует Rigidbody!");
            }

            // Уничтожить пулю через 5 секунд
            Destroy(newBullet, 5f);

            // Ждать интервал стрельбы
            yield return new WaitForSeconds(1 / _buildingData.Production[0]);
        }

        // Сброс ссылки на корутину
        shootingCoroutine = null;
    }
}

