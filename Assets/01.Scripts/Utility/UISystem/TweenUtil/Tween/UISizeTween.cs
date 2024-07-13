using System.Collections;
using UnityEngine;

public class UISizeTween : UITween
{
    public Vector2 fromSize;
    public Vector2 toSize;
    
    public override void SetTween()
    {
        Target.sizeDelta = fromSize;
    }

    protected override IEnumerator PlayRoutine()
    {
        var origin = fromSize;
        var target = toSize;
        var currentTime = 0f;

        while (currentTime <= duration)
        {
            currentTime += Time.unscaledDeltaTime;
            var percent = Easing.Get(ease, currentTime / duration);
            var size = Vector2.Lerp(origin, target, percent);
            Target.sizeDelta = size;
            yield return null;
        }

        Target.sizeDelta = toSize;
    }
}