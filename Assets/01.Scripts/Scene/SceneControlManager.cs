using System;
using Singleton;
using UnityEngine;

public class SceneControlManager : MonoSingleton<SceneControlManager>
{
    [SerializeField] private SceneType _startSceneType;
    
    public Scene CurrentScene { get; private set; }

    [ContextMenu("Test")]
    private void adsfsadf()
    {
        ChangeScene(_startSceneType);
    }

    public void ChangeScene(SceneType nextScene)
    {
        UIManager.Instance.AppearUI(PoolingItemType.SceneTransitionPanel, UIManager.Instance.TopCanvas.transform);
        
        if (CurrentScene != null)
        {
            CurrentScene.ExitScene();
        }

        CurrentScene = PoolManager.Instance.Pop($"{nextScene}Scene") as Scene;
        CurrentScene?.EnterScene();
    }
}