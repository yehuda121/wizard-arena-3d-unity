using UnityEngine;

// This script centralizes control of all animation-related behavior
public class SC_WizardAnimator : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Controls walking animation (true = walking, false = idle)
    public void SetWalking(bool isWalking)
    {
        if (animator != null)
        {
            //Debug.Log("isWalking animator activated");
            //Debug.Log("...");
            animator.SetBool("isWalking", isWalking);
        }
    }

    // Controls shield animation (true = active, false = inactive)
    public void SetShielding(bool isShielding)
    {
        if (animator != null)
        {
            Debug.Log("isShielding animator activated");
            animator.SetBool("isShielding", isShielding);
        }
    }

    // Triggers attack animation
    public void PlayAttack()
    {
        if (animator != null)
        {
            Debug.Log("attac animator activated");
            animator.SetTrigger("attack");
        }
    }

    // Triggers death animation
    public void PlayDeath()
    {
        if (animator != null)
        {
            Debug.Log("die animator activated");
            animator.SetTrigger("die");
        }
    }
}
