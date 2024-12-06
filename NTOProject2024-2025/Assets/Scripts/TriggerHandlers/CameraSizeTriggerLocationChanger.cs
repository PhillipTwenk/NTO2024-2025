using UnityEngine;

public class CameraSizeTriggerLocationChanger : MonoBehaviour
{
    public Camera MainCamera;
    public int ThisLocationCameraSize;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            MainCamera.orthographicSize = ThisLocationCameraSize;
        }
    }
}
