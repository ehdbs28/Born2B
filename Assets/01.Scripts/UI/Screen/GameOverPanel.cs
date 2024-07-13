using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameOverPanel : UIComponent
{
    [SerializeField] private ItemInventorySO _inventory;
    
    private TextMeshProUGUI _stageInfoText;
    private Transform _itemParent;

    protected override void Awake()
    {
        base.Awake();
        _stageInfoText = transform.Find("StageText").GetComponent<TextMeshProUGUI>();
        _itemParent = transform.Find("ItemParent");
    }

    private void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            SceneControlManager.Instance.ChangeScene(SceneType.Title);
        }
    }

    public override void Appear(Transform parent)
    {
        base.Appear(parent);
        
        // _inventory.
    }
}