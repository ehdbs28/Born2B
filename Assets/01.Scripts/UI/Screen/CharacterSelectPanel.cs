using UnityEngine;

public class CharacterSelectPanel : UIComponent
{
    [SerializeField] private PlayerSelectSO _playerSelects;
    
    public void SelectIndex(int index)
    {
        SceneControlManager.Instance.ChangeScene(SceneType.Stage, null, () =>
        {
            ItemManager.Instance.Init(_playerSelects.weaponItemDatabase);
            UnitSelectManager.Instance.Select(index);
        });
    }
}