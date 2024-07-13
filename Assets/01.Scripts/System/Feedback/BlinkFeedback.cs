using System;
using UnityEngine;

public class BlinkFeedback : Feedback
{
    [SerializeField] Renderer targetRenderer = null;
    [SerializeField] Material blinkMaterial = null;
    [SerializeField] float delay = 0.15f;

    private Material originMaterial = null;

    public override void Play(Vector3 playPos)
    {
        originMaterial = targetRenderer.material;
        targetRenderer.material = blinkMaterial;
        StartSafeCoroutine("Blink", this.DelayCoroutine(delay, () => {
            targetRenderer.material = originMaterial;
            StartCoroutine(this.DelayCoroutine(delay, () => {

            }));
        }));
    }

    private void SetBlink(Material materail, Action callback)
    {
        targetRenderer.material = materail;
        StartCoroutine(this.DelayCoroutine(delay, callback));
    }

    public override void Stop()
    {
        base.Stop();
        StopSafeCoroutine("Blink");
    }
}
