public class TitleScene : Scene
{
    public override SceneType Type => SceneType.Title;
    
    public override void LoadedScene()
    {
        UIManager.Instance.AppearUI(PoolingItemType.TitlePanel);
    }
}