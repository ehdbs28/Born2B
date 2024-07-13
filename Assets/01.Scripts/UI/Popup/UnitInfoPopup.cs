using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoPopup : UIComponent
{
    private Transform _statusParent;

    private List<UIComponent> _statusIcons;

    private TextMeshProUGUI _nameText;
    private TextMeshProUGUI _descText;

    private Material _hpSliderMat;
    private List<TextMeshProUGUI> _statValueTexts;

    private UnitDataSO _unitData;

    private readonly int _fillAmountHash = Shader.PropertyToID("_FillAmount");

    protected override void Awake()
    {
        base.Awake();

        _statusIcons = new List<UIComponent>();

        _statusParent = transform.Find("StatusInfo");
        _nameText = transform.Find("Data/InfoText/NameText").GetComponent<TextMeshProUGUI>();
        _descText = transform.Find("Data/InfoText/DescText").GetComponent<TextMeshProUGUI>();
        
        var image = transform.Find("Data/HpSlider/Value").GetComponent<Image>();
        _hpSliderMat = Instantiate(image.material);
        image.material = _hpSliderMat;

        _statValueTexts = new List<TextMeshProUGUI>();
        transform.Find("Data/StatValue").GetComponentsInChildren(_statValueTexts);
    }

    public void Init(UnitDataSO newData)
    {
        _nameText.text = newData.unitName;
        _descText.text = newData.unitDesc;

        _unitData = newData;

        newData.statusController.StatusEffect.OnStatusChanged += StatusHandle;
        newData.health.OnChangedHpEvent += DamagedHandle;
        newData.Stat.OnStatChangedEvent += StatChangedHandle;
        
        StatusHandle();
        DamagedHandle(newData.health.CurrentHp, newData.health.MaxHp);
        StatChangedHandle();
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
        _unitData.Stat.OnStatChangedEvent -= StatChangedHandle;
        base.Disappear(poolIn);
    }

    private void DamagedHandle(int curHp, int maxHp)
    {
        _hpSliderMat.SetFloat(_fillAmountHash, curHp / (float)maxHp);
    }

    private void StatChangedHandle()
    {
        for (var i = 0; i < _unitData.Stat.stats.Count; i++)
        {
            var stat = _unitData.Stat[(StatType)(i + 1)];

            if (stat == null)
            {
                continue;
            }

            _statValueTexts[i].text = ((int)stat.CurrentValue).ToString();
        }
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
            var statusIcon = UIManager.Instance.AppearUI(Enum.Parse<PoolingItemType>($"StatusIconUnit-{effected}"), _statusParent);
            _statusIcons.Add(statusIcon);
        }
    }
    
    public void SetPosition(Vector2 newPos)
    {
        rectTransform.anchoredPosition = newPos;
        AdJustPivot();
    }

    private void AdJustPivot()
    {
        var curPos = rectTransform.anchoredPosition;
        var size = rectTransform.sizeDelta;

        var newPivot = Vector2.zero;

        if (curPos.x + size.x >= Screen.width)
        {
            newPivot.x = 1;
        }
        if (curPos.y + size.y >= Screen.height)
        {
            newPivot.y = 1;
        }
        
        rectTransform.pivot = newPivot;
    }
}