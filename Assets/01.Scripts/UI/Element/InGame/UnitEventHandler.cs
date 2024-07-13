using UnityEngine;
using UnityEngine.EventSystems;

public class UnitEventHandler : UIButton
{
    public UnitDataSO unitData;
    private UnitInfoPopup _infoPopup;
    
    public void ShowInfoUI(PointerEventData eventData)
    {
        if (_infoPopup != null || unitData == null)
        {
            return;
        }
        
        _infoPopup = UIManager.Instance.AppearUI(PoolingItemType.UnitInfoPopup) as UnitInfoPopup;
        _infoPopup.Init(unitData);
    }

    public void UnShowInfoUI(PointerEventData eventData)
    {
        if (_infoPopup == null || unitData == null)
        {
            return;
        }
        
        _infoPopup.Disappear();
        _infoPopup = null;
    }

    public void UpdateInfoUI(PointerEventData eventData)
    {
        if (_infoPopup == null || unitData == null)
        {
            return;
        }

        var mousePosition = eventData.position;
        _infoPopup.SetPosition(mousePosition);
    }
}