public class SelectCharacterScene : Scene
{
    public override SceneType Type => SceneType.SelectCharacter;

    public override void EnterScene()
    {
        UIManager.Instance.AppearUI(PoolingItemType.StartingPlayerSelectPanel);
    }

    public override void OnPop()
    {
    }
}