using System;
using Singleton;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private Canvas _mainCanvas;
    [SerializeField] private Transform _unitInfoMiniParent;
    public Transform UnitINfoMiniParent => _unitInfoMiniParent;

    [SerializeField] private Canvas _topCanvas;

    public Canvas MainCanvas => _mainCanvas;
    public Canvas TopCanvas => _topCanvas;

    public UIComponent AppearUI(PoolingItemType key, Transform parent = null, Action callBack = null)
    {
        var component = PoolManager.Instance.Pop(key) as UIComponent;

        if (parent is null)
        {
            parent = _mainCanvas.transform;
        }

        component.OnAppearEvent += callBack;
        component.Appear(parent);
        return component;
    }
}