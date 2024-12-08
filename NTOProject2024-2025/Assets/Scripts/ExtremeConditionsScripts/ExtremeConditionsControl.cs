using UnityEngine;
using System;
public class ExtremeConditionsManager : MonoBehaviour
{
    public bool IsSafe;
    void Start(){
        
    }

    void Update(){
        if(!IsSafe){

        } 
    }

    private void OnTriggerStay(Collider other) {
        if(other.tag == "BaseHemisphere"){
            IsSafe = true;
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.tag == "BaseHemisphere"){
            IsSafe = false;
        }
    }
}
