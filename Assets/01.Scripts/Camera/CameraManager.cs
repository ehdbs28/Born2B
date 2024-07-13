using System.Collections.Generic;
using Singleton;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    private Camera _mainCamera;
    public Camera MainCamera => _mainCamera;

    [SerializeField] private VirtualCamera _virtualCamera;

    public void Init()
    {
        _mainCamera = Camera.main;
    }

    public void ShakeCam(float intensity)
    {
        _virtualCamera.Shake(intensity);
    }
}