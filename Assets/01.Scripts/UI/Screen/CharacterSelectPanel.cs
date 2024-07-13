public class CharacterSelectPanel : UIComponent
{
    public void SelectIndex(int index)
    {
        UnitSelectManager.Instance.Select(index);
    }
}