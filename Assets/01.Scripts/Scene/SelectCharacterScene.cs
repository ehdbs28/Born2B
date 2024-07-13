public class SelectCharacterScene : Scene
{
    public override SceneType Type => SceneType.SelectCharacter;

    public override void OnPop()
    {
        UIManager.Instance.AppearUI(PoolingItemType.StartingPlayerSelectPanel);
    }

    public override void OnPush()
    {
    }
}