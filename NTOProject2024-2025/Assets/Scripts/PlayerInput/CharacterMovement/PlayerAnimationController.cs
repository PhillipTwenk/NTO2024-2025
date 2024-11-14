using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animController;
    private float t;
    
    private void Start()
    {
        animController = GetComponent<Animator>();
    }
    
    public void Run(float speed,bool status)
    {
        animController.SetBool("Run", status);
        if (status == true)
        {
            if (speed>3)
            {
                t += Time.deltaTime;
                if (t >= 2) { t = 2; }
                animController.SetFloat("Speed", t);
            }
            else
            {
                t -= Time.deltaTime;
                if (t <= 0) { t = 0; }
                animController.SetFloat("Speed", t);
            }
        }
        else
        {
            t = 0;
        }
    }

    public void JumpAnim(string mode, bool IsInAir)
    {
        if (mode == "start")
        {
            animController.SetBool("Jump", true);
            if (IsInAir)
            {
                animController.SetBool("IsInAir", true);
            }
        }else if (mode == "end")
        {
            animController.SetBool("Jump", false);
            animController.SetBool("IsInAir", false);
        }
    }

    public void Falling()
    {
        animController.SetBool("IsInAir",true);
    }
}
