using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameOverPanelItem : UIButton
{
    private ItemSO _holdingItemSo;
    private ItemInfoPopup _infoPopup;

    private Image _innerImage;

    protected override void Awake()
    {
        base.Awake();
        _innerImage = GetComponent<Image>();
    }
    
    public void Init(ArtifactItemSO newItem)
    {
        if (newItem == null)
        {
            return;
        }

        _innerImage.sprite = newItem.ItemIcon;
        _holdingItemSo = newItem;
    }

    public void ShowInfoUI(PointerEventData eventData)
    {
        if (_infoPopup != null || _holdingItemSo == null)
        {
            return;
        }
        
        _infoPopup = UIManager.Instance.AppearUI(PoolingItemType.ItemInfoPopup) as ItemInfoPopup;
        _infoPopup.Init(_holdingItemSo);
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