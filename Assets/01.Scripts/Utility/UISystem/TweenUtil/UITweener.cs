using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class UITweener : IUITweener
{
    public List<UITween> tweens = new List<UITween>();
    private UIComponent _ownerComponent;

    public bool IsPlay { get; private set; }
    public event Action onComplete = null;
    
    public bool IsSet { get; private set; }

    public void Init(UIComponent component)
    {
        _ownerComponent = component;
        foreach (var tween in tweens)
        {
            tween.Init(component);
        }
        IsSet = true;
    }

    public void Release()
    {
        IsSet = false;
    }

    public UITweener PlayTween()
    {
        _ownerComponent.StartSafeCoroutine("TweenerPlayRoutine", PlayRoutine());
        return this;
    }

    public void StopTween()
    {
        onComplete?.Invoke();
        _ownerComponent.StopSafeCoroutine("TweenerPlayRoutine");
        foreach (var tween in tweens)
        {
            tween.StopTween();
        }
    }

    private IEnumerator PlayRoutine()
    {
        IsPlay = true;

        foreach (var tween in tweens)
        {
            if (tween.joinPrevAnimation)
            {
                tween.PlayTween();
                yield return null;
            }
            else
            {
                yield return tween.PlayTween();
            }
        }

        IsPlay = false;
        onComplete?.Invoke();
        onComplete = null;
    }

    public void OnComplete(Action callBack)
    {
        onComplete += callBack;
    }
}