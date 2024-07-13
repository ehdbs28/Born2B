using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoPopupMini : UIComponent
{
    [SerializeField] private Vector2 positionOffset;
    
    public Transform owner;
    
    private Transform _statusParent;

    private Material _hpSliderMat;
    private List<UIComponent> _statusIcons;

    private UnitDataSO _unitData;
    
    private readonly int _fillAmountHash = Shader.PropertyToID("_FillAmount");

    protected override void Awake()
    {
        base.Awake();
        _statusIcons = new List<UIComponent>();
        _statusParent = transform.Find("StatusEffect");
        
        var image = transform.Find("HpSlider/Value").GetComponent<Image>();
        _hpSliderMat = Instantiate(image.material);
        image.material = _hpSliderMat;
    }

    public void Init(UnitDataSO newData, Transform owner)
    {
        this.owner = owner;
        
        _unitData = newData;
        newData.statusController.StatusEffect.OnStatusChanged += StatusHandle;
        _unitData.health.OnChangedHpEvent += DamagedHandle;
        StatusHandle();
        DamagedHandle(_unitData.health.CurrentHp, _unitData.health.MaxHp);
    }

    private void LateUpdate()
    {
        if (owner == null)
        {
            return;
        }

        var ownerScreenPosition = CameraManager.Instance.MainCamera.WorldToScreenPoint(owner.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)UIManager.Instance.MainCanvas.transform, ownerScreenPosition, CameraManager.Instance.MainCamera, out var canvasPos);

        canvasPos += positionOffset;
        rectTransform.localPosition = canvasPos;
    }

    public override void Disappear(bool poolIn = true)
    {
        foreach (var statusIcon in _statusIcons)
        {
            statusIcon.Disappear();
        }
        _statusIcons.Clear();
        
        _unitData.statusController.StatusEffect.OnStatusChanged += StatusHandle;
        _unitData.health.OnChangedHpEvent -= DamagedHandle;
        base.Disappear(poolIn);
    }
    
    private void DamagedHandle(int curHp, int maxHp)
    {
        _hpSliderMat.SetFloat(_fillAmountHash, curHp / (float)maxHp);
    }
    
    private void StatusHandle()
    {
        foreach (var statusIcon in _statusIcons)
        {
            statusIcon.Disappear();
        }
        _statusIcons.Clear();

        var effectedStatus = _unitData.statusController.StatusEffect.EffectedStatus;
        foreach (var effected in effectedStatus)
        {
            var statusIcon = UIManager.Instance.AppearUI(Enum.Parse<PoolingItemType>($"StatusIconMiniUnit-{effected}"), _statusParent);
            _statusIcons.Add(statusIcon);
        }
    }
}