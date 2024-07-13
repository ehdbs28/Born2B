using System;
using System.Collections;
using UnityEngine;

public static class MonoBehaviourExtensions
{
	public static IEnumerator DelayCoroutine(this MonoBehaviour left, float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }

    public static IEnumerator PostponeFrameCoroutine(this MonoBehaviour left, Action callback)
    {
        yield return null;
        callback?.Invoke();
    }
}
