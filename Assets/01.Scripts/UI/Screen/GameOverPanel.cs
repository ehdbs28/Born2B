using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameOverPanel : UIComponent
{
    [SerializeField] private ItemInventorySO _inventory;
    [SerializeField] private List<Image> itemSlots;
    
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
        
        List<ItemSO> items = _inventory.AllItems;
        for(int i = 0; i < itemSlots.Count; ++i)
        {
            if(items.Count >= i)
                itemSlots[i].gameObject.SetActive(false);
            else
            {
                itemSlots[i].sprite = itemSlots[i].sprite;
                itemSlots[i].gameObject.SetActive(true);
            }
        }

        StageDataSO stageData = null;
        ChapterDataSO chapterData = null;
        _stageInfoText.text = $"{chapterData.chapterName} - {stageData.stageName}";
    }
}