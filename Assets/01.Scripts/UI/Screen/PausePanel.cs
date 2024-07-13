using UnityEngine;

public class PausePanel : UIComponent
{
    private UIComponent settingPanel = null;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(settingPanel != null && settingPanel.state == PoolingState.PoolOut)
                settingPanel.Disappear();
            else
                Disappear();
        }
    }

    public override void Appear(Transform parent)
    {
        base.Appear(parent);
    }

    public override void Disappear(bool poolIn = true)
    {
        GameManager.Instance.StopPause();
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
        ContinueHandle();
        SceneControlManager.Instance.ChangeScene(SceneType.Title);
    }
}
