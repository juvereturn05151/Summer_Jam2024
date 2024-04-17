using UnityEngine;

public class HumanAnimatorController : MonoBehaviour
{
    public Animator animatorController;
    
    public void Start()
    {
        animatorController.SetBool("isWalking", false);
    }

    public void StartWalkingAnimation()
    {
        animatorController.SetBool("isWalking", true);
    }

    public void StopWalkingAnimation()
    {
        animatorController.SetBool("isWalking", false);
    }
}
