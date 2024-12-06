using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    public int HP;

    void Start()
    {
        HP = 100;
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Bullet"){
            HP-=50;
        }
    }
}
