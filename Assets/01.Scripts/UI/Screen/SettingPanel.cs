using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : UIComponent
{
    [SerializeField] List<AudioSettingSlot> settingSlots = new List<AudioSettingSlot>();

    public override void Appear(Transform parent)
    {
        base.Appear(parent);
    }

    public override void Disappear(bool poolIn = true)
    {
        base.Disappear(poolIn);
    }
}
