public class TitleScene : Scene
{
    public override SceneType Type => SceneType.Title;

    public override void OnPop()
    {
        UIManager.Instance.AppearUI(PoolingItemType.TitlePanel);
    }

    public override void OnPush()
    {
    }
}