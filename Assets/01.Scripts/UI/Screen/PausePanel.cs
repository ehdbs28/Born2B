using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : UIComponent
{
    public override void Appear(Transform parent)
    {
        base.Appear(parent);    
    }

    public override void Disappear(bool poolIn = true)
    {
        base.Disappear(poolIn);
    }
    
    // events

    public void ContinueHandle()
    {
        
    }

    public void SettingHandle()
    {
        
    }

    public void TitleHandler()
    {
        
    }
}
