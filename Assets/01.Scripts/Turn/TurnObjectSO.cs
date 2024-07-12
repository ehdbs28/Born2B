using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TurnObjectSO : ScriptableObject
{

    [SerializeField] private TurnType _type;
    public TurnType Type => _type;

    public void Init()
    {

        TurnManager.Instance.CommitTurnLogic(_type, TurnLogic, TurnEndLogic);

    }

    public abstract IEnumerator TurnLogic();
    public virtual IEnumerator TurnEndLogic() { yield return null; }

}
