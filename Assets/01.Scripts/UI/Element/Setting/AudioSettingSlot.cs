using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AudioSettingSlot : MonoBehaviour, IPointerMoveHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] AudioGroupType targetGroup;

    [SerializeField] Color activeColor;
    [SerializeField] Color inactiveColor;

    private RectTransform rectTransform = null;

    private bool isDrag = false;
    private List<Image> valueUnits = null;

    private const float MAX_VOLUME = 0f;
    private const float MIN_VOLUME = -40f;
    private const float MUTE_VOLUME = -80f;

    private void Awake()
    {
        rectTransform = transform as RectTransform;

        valueUnits = new List<Image>();
        transform.GetComponentsInChildren<Image>(valueUnits);
        valueUnits.RemoveAt(0);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if(isDrag == false)
            return;

        DisplayValueUnits(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDrag = true;
        DisplayValueUnits(eventData);
    }

    public void OnPointerUp(PointerEventData eventData) => isDrag = false;

    private void DisplayValueUnits(PointerEventData eventData)
    {
        var size = rectTransform.sizeDelta;
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position,
            eventData.pressEventCamera, out var localPos);

        localPos.x += size.x;
        localPos.y += size.y / 2f;

        float ratio = Mathf.Clamp(localPos.x / size.x, 0f, 1f);

        int unitsIndex = Mathf.CeilToInt(Mathf.Lerp(-1, 11, ratio));
        for(int i = 0; i < valueUnits.Count; ++i)
            valueUnits[i].color = i > unitsIndex ? inactiveColor : activeColor;

        float value = Mathf.CeilToInt(Mathf.Lerp(0, 10, ratio)) * 0.1f;
        // Debug.Log(value);
        float volume;
        if(value < 0.1f)
            volume = MUTE_VOLUME;
        else
            volume = Mathf.Lerp(MIN_VOLUME, MAX_VOLUME, value);
        AudioManager.Instance.SetVolume(targetGroup, volume);
    }
}
