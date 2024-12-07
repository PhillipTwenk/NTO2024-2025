using UnityEngine;

public class EnemyDamageSystem : MonoBehaviour
{
    public int damage;
    void Start()
    {
        
    }

    private void OnTriggerStay(Collider other) {
        if(other.name == "Bear"){
            // смерть персонажа
        } else if (other.transform.tag == "Building"){
            // нанесение урона зданию
        } else if (other.transform.tag == "Worker"){
            // смерть рабочего
        }
    }

}
