using System;
using System.Collections;
using UnityEngine;

public class UIComponent : PoolableMono
{
    public UIComponentTweenData tweenData;
    [HideInInspector] public RectTransform rectTransform;

    public bool IsPlayTween => tweenData.appearTweener.IsPlay || tweenData.disappearTweener.IsPlay;

    [Header("Attribute Section")] [SerializeField]
    private UIAppearAttribute _appearAttribute;

    [SerializeField] private UIDisappearAttribute _disappearAttribute;

    [Header("Self Disappear Section")] [SerializeField]
    private bool _selfDisappear;

    [SerializeField] private float _selfDisappearTime;

    public event Action OnAppearEvent = null;
    public event Action OnDisappearEvent = null;

    private Vector3 _originAnchoredPos;
    private Quaternion _originLocalRot;

    protected virtual void Awake()
    {
        rectTransform = (RectTransform)transform;
        _originAnchoredPos = rectTransform.anchoredPosition3D;
        _originLocalRot = rectTransform.localRotation;
    }

    public virtual void Appear(Transform parent)
    {
        if (tweenData)
        {
            tweenData.appearTweener.Init(this);
            tweenData.disappearTweener.Init(this);
        }

        transform.SetParent(parent);
        rectTransform.anchoredPosition3D = _originAnchoredPos;
        transform.localRotation = _originLocalRot;
        transform.localScale = Vector3.one;

        void CompleteCallback()
        {
            OnAppearEvent?.Invoke();
            OnAppearEvent = null;
            if (_selfDisappear)
            {
                StartSafeCoroutine("SelfDisappearRoutine", SelfDisappearRoutine());
            }
        }

        ResetPosition((_appearAttribute & UIAppearAttribute.ResetPosX) != 0,
            (_appearAttribute & UIAppearAttribute.ResetPosY) != 0);

        if (tweenData && (_appearAttribute & UIAppearAttribute.UseAnimation) != 0)
        {
            tweenData.disappearTweener.StopTween();
            tweenData.appearTweener.PlayTween().OnComplete(CompleteCallback);
        }
        else
        {
            CompleteCallback();
        }
    }

    public virtual void Disappear(bool poolIn = true)
    {
        void CompleteCallback()
        {
            OnDisappearEvent?.Invoke();
            OnDisappearEvent = null;

            if (tweenData)
            {
                tweenData.appearTweener.Release();
                tweenData.disappearTweener.Release();
            }

            if (poolIn)
            {
                PoolManager.Instance.Push(this);
            }
        }

        if (tweenData && (_disappearAttribute & UIDisappearAttribute.UseAnimation) != 0)
        {
            tweenData.appearTweener.StopTween();
            tweenData.disappearTweener.PlayTween().OnComplete(CompleteCallback);
        }
        else
        {
            CompleteCallback();
        }
    }

    private void ResetPosition(bool resetX, bool resetY)
    {
        var pos = rectTransform.anchoredPosition;
        if (resetX)
            pos.x = 0;
        if (resetY)
            pos.y = 0;
        rectTransform.anchoredPosition = pos;
    }

    private IEnumerator SelfDisappearRoutine()
    {
        yield return new WaitForSeconds(_selfDisappearTime);
        Disappear();
    }

    public override void OnPop()
    {
    }

    public override void OnPush()
    {
    }
}