public class SelectCharacterScene : Scene
{
    public override SceneType Type => SceneType.SelectCharacter;

    public override void LoadedScene()
    {
        UIManager.Instance.AppearUI(PoolingItemType.StartingPlayerSelectPanel);
    }
}