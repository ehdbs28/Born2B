using Singleton;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public bool InPause { get; private set; }

    private void Awake()
    {
        StageManager.Instance.Init();
        CellObjectManager.Instance.Init();
        TurnManager.Instance.Init();
        GetComponent<DebugFlowHandler>().Init(); // 이건 수정할 거임        
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
    }

    public void StopPause()
    {
        if (!InPause)
        {
            return;
        }

        InPause = false;
        Time.timeScale = 1f;
    }
}
