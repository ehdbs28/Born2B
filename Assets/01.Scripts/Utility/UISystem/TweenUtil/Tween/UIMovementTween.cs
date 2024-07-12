using System.Collections;
using UnityEngine;

public class UIMovementTween : UITween
{
    public Vector2 from;
    public Vector2 to;

    public bool startInCurrentPosition;

    public override void SetTween()
    {
        if (!startInCurrentPosition)
        {
            Target.anchoredPosition = from;
        }
    }

    protected override IEnumerator PlayRoutine()
    {
        var currentTime = 0f;

        var origin = startInCurrentPosition ? Target.anchoredPosition : from;

        while (currentTime <= duration)
        {
            currentTime += Time.unscaledDeltaTime;
            var percent = Easing.Get(ease, currentTime / duration);
            var position = Vector2.Lerp(origin, to, percent);
            Target.anchoredPosition = position;
            yield return null;
        }

        Target.anchoredPosition = to;
    }
}