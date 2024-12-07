using UnityEngine;
using System;
public class EnemyHealthSystem : MonoBehaviour
{
    public int HP;

    void Start()
    {
        HP = 100;
    }
    
    void Update(){
        if(HP <= 0){
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Bullet"){
            HP -= 50;
            Debug.Log(other.gameObject.tag);
        }
    }
}
