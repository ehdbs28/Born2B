using System;
using Singleton;
using UnityEngine;

public class SceneControlManager : MonoSingleton<SceneControlManager>
{
    [SerializeField] private SceneType _startSceneType;
    
    public Scene CurrentScene { get; private set; }

    public void Init()
    {
        ChangeScene(_startSceneType);
    }

    public void ChangeScene(SceneType nextScene, Action transitionCompleteEvent = null, Action loadCompleteEvent = null)
    {
        var transitionPanel = UIManager.Instance.AppearUI(PoolingItemType.SceneTransitionPanel, UIManager.Instance.TopCanvas.transform) as TransitionScreen;
        transitionPanel.OnTransitionCompleteEvent += () =>
        {
            if (CurrentScene != null)
            {
                CurrentScene.ExitScene();
            }
            CurrentScene = PoolManager.Instance.Pop($"{nextScene}Scene") as Scene;
            CurrentScene.LoadedScene();
            transitionCompleteEvent?.Invoke();
        };
        transitionPanel.OnLoadCompleteEvent += () =>
        {
            CurrentScene.EnterScene();
            loadCompleteEvent?.Invoke();
        };
    }
}