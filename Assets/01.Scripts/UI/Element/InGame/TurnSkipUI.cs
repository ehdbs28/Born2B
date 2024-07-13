using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnSkipUI : UIComponent
{
    [SerializeField] UIButton skipButton;
    private Image buttonImage = null;

    [Space(15f)]
    [SerializeField] Color enableColor;
    [SerializeField] Color unenableColor;

    private TurnType currentType;

    public void Init()
    {
        buttonImage = skipButton.GetComponent<Image>();
        EventManager.Instance.RegisterEvent(EventType.OnTurnChanged, HandleTurnChanged);
    }

    public void Release()
    {

    }

    private void HandleTurnChanged(object[] args)
    {
        TurnType prevTurn = (TurnType)args[0];
        TurnType nextTurn = (TurnType)args[1];
        currentType = nextTurn;

        if((TurnType.PlayerAttack | TurnType.PreviewCell).HasFlag(currentType))
            buttonImage.color = enableColor;
        else
            buttonImage.color = unenableColor;
    }

    public void HandleSkip()
    {
        if(currentType == TurnType.PlayerAttack)
            SkipAttack();
        else if(currentType == TurnType.PreviewCell)
            SkipMove();
    }

    private void SkipAttack()
    {
        // TurnManager.Instance.EndCurrentTurn();
    }

    private void SkipMove()
    {
        // TurnManager.Instance.EndCurrentTurn();
        // TurnManager.Instance.EndCurrentTurn();
    }
}
