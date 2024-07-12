using System;
using UnityEngine;
using static Controls;

[CreateAssetMenu(menuName = "SO/Input/AttackInputSO")]
public class AttackInputSO : InputSO, IAttackActions
{
    public event Action OnAttackEvent = null;
    public Vector2 ScreenPosition { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();

        AttackActions attack = InputManager.controls.Attack;
        attack.SetCallbacks(this);
        InputManager.RegistInputMap(this, attack.Get());
    }

    public void OnAttack(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if(context.started)
            OnAttackEvent?.Invoke();
    }

    public void OnScreenPosition(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        ScreenPosition = context.ReadValue<Vector2>();
    }
}