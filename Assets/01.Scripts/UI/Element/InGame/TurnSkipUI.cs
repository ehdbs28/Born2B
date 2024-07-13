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

    private void HandleTurnChanged(object[] args)
    {
        TurnType prevTurn = (TurnType)args[0];
        TurnType nextTurn = (TurnType)args[1];
        currentType = nextTurn;
    }

    public void HandleSkip()
    {
        buttonImage.color = enableColor;
        if(currentType == TurnType.PlayerAttack)
            SkipAttack();
        else if(currentType == TurnType.PreviewCell)
            SkipMove();
        else
            buttonImage.color = unenableColor;

    }

    private void SkipAttack()
    {
        TurnManager.Instance.EndCurrentTurn();
    }

    private void SkipMove()
    {
        TurnManager.Instance.EndCurrentTurn();
        TurnManager.Instance.EndCurrentTurn();
    }
}
