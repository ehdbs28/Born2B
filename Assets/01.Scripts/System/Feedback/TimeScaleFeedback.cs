using UnityEngine;

public class TimeScaleFeedback : Feedback
{
    [SerializeField] int priority = 1;
    [SerializeField] float delay = 0;
    [SerializeField] float duration = 0;
    [SerializeField] float value = 0;

    private static float originValue = -1;
    private static int currentRunningPriority = 0;

    public override void Play(Vector3 playPos)
    {
        if(priority < currentRunningPriority)
            return;
        currentRunningPriority = priority;

        StopSafeCoroutine("return");
        if(originValue == -1)
            originValue = Time.timeScale;

        StartSafeCoroutine("main", this.DelayCoroutine(delay, () => {
            Time.timeScale = value;
            StartSafeCoroutine("return", this.DelayCoroutine(duration, () => {
                Time.timeScale = originValue;
                originValue = -1;
                currentRunningPriority = 0;
            }));
        }));
    }
}
