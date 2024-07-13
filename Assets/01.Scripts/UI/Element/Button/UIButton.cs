using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIButton : UIComponent, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public UnityEvent<PointerEventData> onClickEvent;
    public UnityEvent<PointerEventData> onPointerEnterEvent;
    public UnityEvent<PointerEventData> onPointerExitEvent;
    public UnityEvent<PointerEventData> onPointerMoveEvent;

    private Vector3 _originScale;

    private bool _isHover;

    [SerializeField] private Transform _targetImageTrm;

    protected override void Awake()
    {
        base.Awake();
        _originScale = transform.localScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClickEvent?.Invoke(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isHover)
        {
            return;
        }

        _isHover = true;
        
        if (_targetImageTrm != null)
        {
            StartSafeCoroutine(nameof(ScaleChangeRoutine), ScaleChangeRoutine(_originScale * 1.1f, 0.1f));
        }
        onPointerEnterEvent?.Invoke(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_isHover)
        {
            return;
        }

        _isHover = false;
        
        if (_targetImageTrm != null)
        {
            StartSafeCoroutine(nameof(ScaleChangeRoutine), ScaleChangeRoutine(_originScale, 0.1f));
        }
        onPointerExitEvent?.Invoke(eventData);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        onPointerMoveEvent?.Invoke(eventData);
    }

    private IEnumerator ScaleChangeRoutine(Vector3 targetScale, float time)
    {
        var origin = _targetImageTrm.localScale;
        var current = 0f;

        while (current <= time)
        {
            current += Time.deltaTime;
            var percent = current / time;
            var scale = Vector3.Lerp(origin, targetScale, percent);
            _targetImageTrm.localScale = scale;
            yield return null;
        }

        _targetImageTrm.localScale = targetScale;
    }
}