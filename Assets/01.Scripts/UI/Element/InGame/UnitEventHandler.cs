using UnityEngine;
using UnityEngine.EventSystems;

public class UnitEventHandler : UIButton
{
    [SerializeField] private UnitDataSO _unitData;
    private UnitInfoPopup _infoPopup;
    
    public void ShowInfoUI(PointerEventData eventData)
    {
        if (_infoPopup != null || _unitData == null)
        {
            return;
        }
        
        _infoPopup = UIManager.Instance.AppearUI(PoolingItemType.UnitInfoPopup) as UnitInfoPopup;
        _infoPopup.Init(_unitData);
    }

    public void UnShowInfoUI(PointerEventData eventData)
    {
        if (_infoPopup == null)
        {
            return;
        }
        
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