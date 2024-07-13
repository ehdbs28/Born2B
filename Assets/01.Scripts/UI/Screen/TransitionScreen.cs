using System;
using System.Collections;
using UnityEngine;

public class TransitionScreen : UIComponent
{
    private Animator _animator;

    private readonly int _inTransitionHash = Animator.StringToHash("InTransition");
    private readonly int _outTransitionHash = Animator.StringToHash("OutTransition");

    protected override void Awake()
    {
        base.Awake();
        _animator = transform.GetComponent<Animator>();
    }

    public override void Appear(Transform parent)
    {
        base.Appear(parent);
        StartSafeCoroutine("InTransition", AnimationWaitRoutine(_inTransitionHash));
    }

    public override void Disappear(bool poolIn = true)
    {
        StartSafeCoroutine("OutTransition", AnimationWaitRoutine(_outTransitionHash, () =>
        {
            base.Disappear(poolIn);
        }));
    }

    private IEnumerator AnimationWaitRoutine(int hash, Action callBack = null)
    {
        _animator.Play(hash, -1, 0f);

        yield return new WaitForEndOfFrame();

        var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        while (stateInfo.shortNameHash == hash && stateInfo.normalizedTime < 1f)
        {
            yield return null;
            stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        }

        callBack?.Invoke();
    }
}