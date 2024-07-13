using TMPro;
using UnityEngine;

public class DamageTextPop : UIComponent
{
    private TextMeshProUGUI _text;
    
    [SerializeField] private Gradient _damageGradient;
    [SerializeField] private float _minSize;
    [SerializeField] private float _maxSize;

    private const float MinDam = 10f;
    private const float MaxDam = 100f;

    private Transform _tempParent;

    protected override void Awake()
    {
        base.Awake();
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void Init(Vector3 worldPosition, float damage)
    {
        var ownerScreenPosition = CameraManager.Instance.MainCamera.WorldToScreenPoint(worldPosition);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)UIManager.Instance.MainCanvas.transform, ownerScreenPosition, CameraManager.Instance.MainCamera, out var canvasPos);
        rectTransform.anchoredPosition = canvasPos;
        
        var percent = Mathf.Clamp(damage, MinDam, MaxDam) / MaxDam;

        var color = _damageGradient.Evaluate(percent);
        var size = Mathf.Lerp(_minSize, _maxSize, percent);

        _text.color = color;
        _text.fontSize = size;

        _text.text = $"+{damage}";

        tweenData.appearTweener.PlayTween();
    }
}