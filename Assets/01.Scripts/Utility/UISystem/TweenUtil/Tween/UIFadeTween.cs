using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeTween : UITween
{
    private Image _image;

    public bool fadeIn;

    public override void Init(UIComponent component)
    {
        base.Init(component);
        _image = Target.GetComponent<Image>();
    }

    public override void SetTween()
    {
        SetAlpha(fadeIn ? 0f : 1f);
    }

    protected override IEnumerator PlayRoutine()
    {
        var origin = fadeIn ? 0f : 1f;
        var target = fadeIn ? 1f : 0f;
        var currentTime = 0f;

        while (currentTime <= duration)
        {
            currentTime += Time.unscaledDeltaTime;
            var percent = Easing.Get(ease, currentTime / duration);
            var alpha = Mathf.Lerp(origin, target, percent);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(target);
    }

    private void SetAlpha(float alpha)
    {
        if (_image is null)
        {
            return;
        }

        var color = _image.color;
        color.a = alpha;
        _image.color = color;
    }
}