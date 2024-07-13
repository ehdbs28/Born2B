using UnityEngine;

public class PausePanel : UIComponent
{
    private UIComponent settingPanel = null;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(settingPanel != null)
                return;
            Disappear();
        }
    }

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
        settingPanel = UIManager.Instance.AppearUI(PoolingItemType.SettingPanel);
    }

    public void TitleHandler()
    {
        // 인게임 지우기
    }
}
