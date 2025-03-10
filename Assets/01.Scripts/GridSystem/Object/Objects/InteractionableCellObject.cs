using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionableCellObject : CellObjectInstance
{


    protected virtual void OnTriggerEnter2D(Collider2D other)
    {

        if (TurnManager.Instance.GetTurnData<bool>(TurnDataType.IsMovementCell))
        {

            if(other.TryGetComponent<CellObjectInstance>(out var ins))
            {

                Interaction(ins);

            }

        }

    }


    protected abstract void Interaction(CellObjectInstance interactionInstance);

}
