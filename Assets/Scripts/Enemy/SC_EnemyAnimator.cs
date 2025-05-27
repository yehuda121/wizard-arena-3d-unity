using UnityEngine;

public class SC_EnemyAnimator : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetWalking(bool isWalking)
    {
        if (animator != null)
            animator.SetBool("isWalking", isWalking);
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
