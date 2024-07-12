using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    public ItemRarity Rarity;
    public Sprite ItemIcon;
    public string ItemName;

	public abstract bool Execute(IItemHandler handler);
    public abstract bool Unexecute(IItemHandler handler);
    protected bool TryParseHandler<T>(IItemHandler inHandler, out T outHandler) where T : class, IItemHandler
    {
        outHandler = inHandler as T;
        if (outHandler == null)
            return false;
        return true;
    }
}
