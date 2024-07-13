using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : UIComponent
{
    [SerializeField] List<AudioSettingSlot> settingSlots = new List<AudioSettingSlot>();
}
