public class StageScene : Scene
{
    public override SceneType Type => SceneType.Stage;

    public override void OnPop()
    {
        UIManager.Instance.AppearUI(PoolingItemType.InGamePanel);
    }

    public override void OnPush()
    {
    }
}