using UnityEngine;

public class TriggerAnimationFeedback : Feedback
{
    [SerializeField] Animator animator;
    [SerializeField] string paramName;

    public override void Play(Vector3 playPos)
    {
        animator.SetTrigger(paramName);
    }
}
