public class StageScene : Scene
{
    public override SceneType Type => SceneType.Stage;

    public override void LoadedScene()
    {
        UIManager.Instance.AppearUI(PoolingItemType.InGamePanel);
    }

    public override void EnterScene()
    {
        UIManager.Instance.AppearUI(PoolingItemType.StageEnterPopup);
        FlowManager.Instance.StartFlow();
    }
}