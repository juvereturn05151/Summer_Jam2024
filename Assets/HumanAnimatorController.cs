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

    public void StartDeadAnimation()
    {
        animatorController.SetBool("isDead", true);
    }

    public void StartWinAnimation()
    {
        animatorController.SetBool("isWin", true);
    }

    public void StartHurtAnimation()
    {
        animatorController.SetBool("isHurt", true);
    }

    public void StopHurtAnimation()
    {
        animatorController.SetBool("isHurt", false);
    }
}
