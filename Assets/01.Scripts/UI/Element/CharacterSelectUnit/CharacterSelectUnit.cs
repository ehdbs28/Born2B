using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterSelectUnit : UIButton
{
    private TextMeshProUGUI _cursorText;

    protected override void Awake()
    {
        base.Awake();
        _cursorText = transform.Find("CursorText").GetComponent<TextMeshProUGUI>();
    }

    public override void Appear(Transform parent)
    {
        base.Appear(parent);
        HoverEnd(null);
    }

    public void Hover(PointerEventData eventData)
    {
        _cursorText.gameObject.SetActive(true);
    }

    public void HoverEnd(PointerEventData eventData)
    {
        _cursorText.gameObject.SetActive(false);
    }
}