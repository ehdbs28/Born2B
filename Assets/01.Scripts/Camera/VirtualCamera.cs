using Cinemachine;
using UnityEngine;

public class VirtualCamera : ExtendedMono
{
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineImpulseSource _impulseSource;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake(float intensity = 1f)
    {
        if (_impulseSource == null)
        {
            return;
        }

        _impulseSource.GenerateImpulse(intensity);
    }
}