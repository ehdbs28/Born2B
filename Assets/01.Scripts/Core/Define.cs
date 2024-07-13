using UnityEngine;

public static class UIDefine
{
    public static string GetRarityKoreanText(ItemRarity rarity)
    {
        return rarity switch
        {
            ItemRarity.Common => "커먼",
            ItemRarity.Rare => "레어",
            ItemRarity.Epic => "에픽",
            _ => ""
        };
    }
    
    public static Color GetColorByRarity(ItemRarity rarity)
    {
        return rarity switch
        {
            ItemRarity.Common => new Color(0f, 157f / 255f, 0f, 1f),
            ItemRarity.Rare => Color.blue,
            ItemRarity.Epic => new Color(135f / 255f, 0f, 185 / 255f, 1f),
            _ => Color.white
        };
    }
}