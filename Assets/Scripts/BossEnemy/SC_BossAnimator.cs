using UnityEngine;

public class SC_BossAnimator : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
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
