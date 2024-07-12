using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeObject : InteractionableCellObject
{
    private const float SPIKE_DAMAGE_RATIO = 1f / 10f;
    
    protected override void Interaction(CellObjectInstance interactionInstance)
    {

        if(interactionInstance is IHitable)
        {
            if(interactionInstance.TryGetComponent<IHealth>(out IHealth ih))
                (interactionInstance as IHitable).Hit(null, ih.MaxHp * SPIKE_DAMAGE_RATIO, false);

        }

    }

}
