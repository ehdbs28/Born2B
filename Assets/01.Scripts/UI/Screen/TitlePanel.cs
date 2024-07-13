using UnityEngine;

public class TitlePanel : UIComponent
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
    public void PlayHandle()
    {
        SceneControlManager.Instance.ChangeScene(SceneType.SelectCharacter);
    }

    public void SettingHandle()
    {
        UIManager.Instance.AppearUI(PoolingItemType.SettingPanel);
    }

    public void QuitHandle()
    {
        Application.Quit();
    }
}
