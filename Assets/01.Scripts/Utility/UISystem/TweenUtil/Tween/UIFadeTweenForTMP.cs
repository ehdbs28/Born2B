using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeTweenForTMP : UITween
{
    private TextMeshProUGUI _text;

    public bool fadeIn;

    public override void Init(UIComponent component)
    {
        base.Init(component);
        _text = Target.GetComponent<TextMeshProUGUI>();
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
        if (_text is null)
        {
            return;
        }

        var color = _text.color;
        color.a = alpha;
        _text.color = color;
    }
}