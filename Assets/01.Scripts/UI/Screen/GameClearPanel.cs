using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameClearPanel : UIComponent
{
    [SerializeField] private ItemInventorySO _inventory;
    [SerializeField] private List<Image> itemSlots;

    private TextMeshProUGUI _stageTitleText;
    private TextMeshProUGUI _stageInfoText;
    private Transform _itemParent;

    protected override void Awake()
    {
        base.Awake();
        _stageTitleText = transform.Find("TitleText").GetComponent<TextMeshProUGUI>();
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

        string valueText;
        switch(UnitSelectManager.Instance.selectedIdx)
        {
            case 0:
                valueText = "<color=#FFAA53>소드마스터</color>";
                break;
            case 1:
                valueText = "<color=#4FFF8E>신궁</color>";
                break;
            case 2:
                valueText = "<color=#7FB6FF>대마법사</color>";
                break;
            default:
                Debug.LogError($"Error : {UnitSelectManager.Instance.selectedIdx} is un defined");
                valueText = "null";
                break;
        }

        _stageTitleText.text = $"BORN TO BE : {valueText}";
        _stageInfoText.text = $"당신에게 이끌린 운명은 {{ {valueText} }} 입니다.";

        List<ItemSO> items = _inventory.AllItems;
        for (int i = 0; i < itemSlots.Count; ++i)
        {
            if (items.Count <= i)
                itemSlots[i].gameObject.SetActive(false);
            else
            {
                itemSlots[i].gameObject.SetActive(true);
                itemSlots[i].sprite = items[i].ItemIcon;
            }
        }

        StageDataSO stageData = null;
        ChapterDataSO chapterData = null;
        // _stageInfoText.text = $"{chapterData.chapterName} - {stageData.stageName}";
    }
}
