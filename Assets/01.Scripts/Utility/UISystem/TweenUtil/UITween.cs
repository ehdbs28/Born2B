using System.Collections;
using UnityEngine;

public class UITween : ScriptableObject, IUITween
{
    private UIComponent _ownerComponent;

    public float duration;
    public Ease ease;

    public string targetTrmPath;
    public bool joinPrevAnimation;

    protected RectTransform Target;

    public virtual void Init(UIComponent component)
    {
        _ownerComponent = component;
        Target = (RectTransform)(targetTrmPath == "" ? component.transform : component.transform.Find(targetTrmPath));
        if (Target is null)
        {
            Debug.LogError($"[UITween] transform path is invalid ({targetTrmPath})");
        }
    }

    public virtual void SetTween()
    {
    }

    public Coroutine PlayTween()
    {
        SetTween();
        return _ownerComponent.StartCoroutine(PlayRoutine());
    }

    public void StopTween()
    {
        _ownerComponent.StopSafeCoroutine("TweenPlayRoutine");
    }

    protected virtual IEnumerator PlayRoutine()
    {
        yield break;
    }
}