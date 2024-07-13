using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalObject : InteractionableCellObject
{

    [SerializeField] private PotalType _potalType;
    [SerializeField] private AudioData _data;
    private List<CellObjectInstance> _areadyInteractionObjects = new();
    private PortalObject _connectedPotal;

    public override void Init(CellObjectSO so)
    {

        base.Init(so);
        var casted = so as PortalSO;
        _potalType = casted.potalType;

    }

    public void Connect(PortalObject connectPortal)
    {

        _connectedPotal = connectPortal;

    }

    public bool CheckTpable()
    {

        return _areadyInteractionObjects.Count <= 0;

    }

    public void Tp(CellObjectInstance ins)
    {

        CellObjectManager.Instance.ChangePosition(ins.key, GetData().position);
        CellObjectManager.Instance.DestroyCloneObjects(ins.key, ins.dataKey);
        ins.transform.localPosition = Vector3.zero;
        _areadyInteractionObjects.Add(ins);

    }

    protected override void Update()
    {

        base.Update();

    }

    protected override void Interaction(CellObjectInstance interactionInstance)
    {

        if (_areadyInteractionObjects.Contains(interactionInstance)) return;

        AudioManager.Instance.PlayAudio(_data);

        if (_potalType == PotalType.TP) 
        {

            if (!_connectedPotal.CheckTpable()) return;
            _connectedPotal.Tp(interactionInstance);

        }
        else
        {

            TurnManager.Instance.SetTurnData(TurnDataType.IsPreview, true);
            FlowManager.Instance.InitCycle();

        }

    }



    private void OnTriggerExit2D(Collider2D collision)
    {

        if (TurnManager.Instance.GetTurnData<bool>(TurnDataType.IsMovementCell))
        {

            if (collision.TryGetComponent<CellObjectInstance>(out var ins))
            {

                _areadyInteractionObjects.Remove(ins);

            }

        }

    }

    public enum PotalType
    {

        TP,
        NextStage

    }

}
