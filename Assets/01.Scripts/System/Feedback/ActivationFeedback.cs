using UnityEngine;

public class ActivationFeedback : Feedback
{
    [SerializeField] GameObject targetObject = null;
    [SerializeField] float turnOffTime = 1f;

    public override void Play(Vector3 playPos)
    {
        StopAllCoroutines();

        targetObject.SetActive(true);
        StartCoroutine(this.DelayCoroutine(turnOffTime, () => targetObject.SetActive(false)));
    }
}
