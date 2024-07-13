using UnityEngine;

public class PausePanel : UIComponent
{
    public override void Appear(Transform parent)
    {
        base.Appear(parent);
    }

    public override void Disappear(bool poolIn = true)
    {
        base.Disappear(poolIn);
    }
    
    // events

    public void ContinueHandle()
    {
        Disappear();
    }

    public void SettingHandle()
    {
        UIManager.Instance.AppearUI(PoolingItemType.SettingPanel);
    }

    public void TitleHandler()
    {
        // 인게임 지우기
    }
}
