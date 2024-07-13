using System;
using UnityEngine;

public class BlinkFeedback : Feedback
{
    [SerializeField] Renderer targetRenderer = null;
    [SerializeField] Material blinkMaterial = null;
    [SerializeField] float delay = 0.15f;
    [SerializeField] int blinkCount = 2;

    private int counter = 0;
    private Material originMaterial = null;

    public override void Play(Vector3 playPos)
    {
        counter = 0;
        originMaterial = targetRenderer.material;

        BlinkLoop();
    }

    private void BlinkLoop()
    {
        counter++;
        SetBlink(blinkMaterial, () => {
            SetBlink(originMaterial, counter >= blinkCount ? null : BlinkLoop);
        });
    }

    private void SetBlink(Material materail, Action callback)
    {
        targetRenderer.material = materail;
        if(callback != null)
            StartSafeCoroutine("Blink", this.DelayCoroutine(delay, callback));
    }

    public override void Stop()
    {
        base.Stop();
        StopSafeCoroutine("Blink");
        targetRenderer.material = originMaterial;
    }
}
