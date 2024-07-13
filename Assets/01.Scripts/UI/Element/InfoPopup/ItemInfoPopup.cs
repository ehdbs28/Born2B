using TMPro;
using UnityEngine;

public class ItemInfoPopup : UIComponent
{
    private TextMeshProUGUI _nameText;
    private TextMeshProUGUI _rarityText;
    private TextMeshProUGUI _descText;

    protected override void Awake()
    {
        base.Awake();

        _nameText = transform.Find("Data/InfoText/NameText").GetComponent<TextMeshProUGUI>();
        _rarityText = transform.Find("Data/InfoText/RarityText").GetComponent<TextMeshProUGUI>();
        _descText = transform.Find("Data/DescText").GetComponent<TextMeshProUGUI>();
    }

    public void Init(ItemSO data)
    {
        _nameText.text = data.ItemName;
        _rarityText.color = UIDefine.GetColorByRarity(data.Rarity);
        _rarityText.text = UIDefine.GetRarityKoreanText(data.Rarity);
        _descText.text = data.ItemDescription;
    }

    public void SetPosition(Vector2 newPos)
    {
        rectTransform.anchoredPosition = newPos;
        AdJustPivot();
    }

    private void AdJustPivot()
    {
        var curPos = rectTransform.anchoredPosition;
        var size = rectTransform.sizeDelta;

        var newPivot = Vector2.zero;

        if (curPos.x + size.x >= Screen.width)
        {
            newPivot.x = 1;
        }
        if (curPos.y + size.y >= Screen.height)
        {
            newPivot.y = 1;
        }
        
        rectTransform.pivot = newPivot;
    }
}