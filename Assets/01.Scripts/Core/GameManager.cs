using Singleton;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public bool InPause { get; private set; }

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
