using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUnitHolder : UIButton
{
    private ItemInfoPopup _infoPopup;
    
    public void ShowInfoUI(PointerEventData eventData)
    {
        _infoPopup = UIManager.Instance.AppearUI(PoolingItemType.ItemInfoPopup) as ItemInfoPopup;
    }

    public void UnShowInfoUI(PointerEventData eventData)
    {
        _infoPopup.Disappear();
        _infoPopup = null;
    }

    public void UpdateInfoUI(PointerEventData eventData)
    {
        if (_infoPopup == null)
        {
            return;
        }

        var mousePosition = eventData.position;
        _infoPopup.SetPosition(mousePosition);
    }
}