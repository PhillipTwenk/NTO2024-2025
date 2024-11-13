using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    public Transform target;
    private Vector3 targetPos;
    public Vector3 offsetPos;
    public float moveSpeed = 5;
    public float smooth = 0.2f;
    private Vector3 velocity = Vector3.zero;
   
    void LateUpdate()
    {
        MoveWithTarget();
    }
    void MoveWithTarget()
    {
        targetPos = target.transform.position + offsetPos;
        //transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime* smooth);
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smooth);
    }
}
