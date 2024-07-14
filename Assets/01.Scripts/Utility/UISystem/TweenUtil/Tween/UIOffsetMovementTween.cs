using System.Collections;
using UnityEngine;

public class UIOffsetMovementTween : UITween
{
    public Vector2 targetOffset;
    
    public override void SetTween()
    {
    }

    protected override IEnumerator PlayRoutine()
    {
        var origin = Target.anchoredPosition;
        var target = origin + targetOffset;

        var current = 0f;

        while (current <= duration)
        {
            current += Time.unscaledDeltaTime;
            var percent = Easing.Get(ease, current / duration);
            var position = Vector2.Lerp(origin, target, percent);
            Target.anchoredPosition = position;
            yield return null;
        }

        Target.anchoredPosition = target;
    }
}