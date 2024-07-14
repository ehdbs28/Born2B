using System;
using Singleton;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public bool InPause { get; private set; }
    private InputMapType prevInputType;

    private void Awake()
    {
        PoolManager.Instance.Init();
        AudioManager.Instance.Init();
        CameraManager.Instance.Init();
        
        StageManager.Instance.Init();
        CellObjectManager.Instance.Init();
        TurnManager.Instance.Init();
        GetComponent<FlowHandler>().Init(); // 이건 수정할 거임     
        
        SceneControlManager.Instance.Init();
    }

    private void Start()
    {
        EventManager.Instance.RegisterEvent(EventType.StageClear, args => 
        {
            UIManager.Instance.AppearUI(PoolingItemType.GameClearPanel);
        });
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(InPause)
                return;

            Pause();
        }
    }

    public void Pause()
    {
        if (InPause)
        {
            return;
        }

        InPause = true;
        Time.timeScale = 0f;
        UIManager.Instance.AppearUI(PoolingItemType.PausePanel);

        prevInputType = InputManager.CurrentInputMapType;
        InputManager.ChangeInputMap(InputMapType.UI);
    }

    public void StopPause()
    {
        if (!InPause)
        {
            return;
        }

        InPause = false;
        Time.timeScale = 1f;
        InputManager.ChangeInputMap(prevInputType);
    }
}
