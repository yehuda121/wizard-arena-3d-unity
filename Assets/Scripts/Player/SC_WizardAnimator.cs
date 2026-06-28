using UnityEngine;

// This script centralizes control of all animation-related behavior
public class SC_WizardAnimator : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void SetWalking(bool isWalking)
    {
        if (animator != null)
            animator.SetBool("isWalking", isWalking);
    }

    public void SetShielding(bool isShielding)
    {
        if (animator != null)
            animator.SetBool("isShielding", isShielding);
    }

    public void PlayAttack()
    {
        if (animator != null)
            animator.SetTrigger("attack");
    }

    public void PlayDeath()
    {
        if (animator != null)
            animator.SetTrigger("die");
    }
}
