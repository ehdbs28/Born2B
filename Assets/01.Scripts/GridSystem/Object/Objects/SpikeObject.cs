using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeObject : InteractionableCellObject
{
    private List<CellObjectInstance> _areadyInteractionObjects = new();
    private const float SPIKE_DAMAGE_RATIO = 1f / 10f;
    private SpikeAnimator _spikeAnime;

    protected override void Awake()
    {

        base.Awake();
        _spikeAnime = GetComponent<SpikeAnimator>();

    }

    protected override void Interaction(CellObjectInstance interactionInstance)
    {

        if (_areadyInteractionObjects.Contains(interactionInstance)) return;

        if (interactionInstance is IHitable)
        {

            _spikeAnime.SetAnimation(false);
            _areadyInteractionObjects.Add(interactionInstance);
            if(interactionInstance.TryGetComponent<IHealth>(out IHealth ih))
                (interactionInstance as IHitable).Hit(null, ih.MaxHp * SPIKE_DAMAGE_RATIO, false);

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if(collision.TryGetComponent<CellObjectInstance>(out var compo) && TurnManager.Instance.GetTurnData<bool>(TurnDataType.IsMovementCell))
        {

            if(compo is IHitable)
            {

                _areadyInteractionObjects.Remove(compo);
                _spikeAnime.SetAnimation(true);

            }

        }

    }


}
