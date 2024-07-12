using System;
using UnityEngine;
using static Controls;

[CreateAssetMenu(menuName = "SO/Input/GridInputSO")]
public class GridInputSO : InputSO, IGridActions
{
    public event Action OnGridStartEvent = null;
    public event Action OnGridEndEvent = null;

    public Vector3 ScreenPosition { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();

        GridActions grid = InputManager.controls.Grid;
        grid.SetCallbacks(this);
        InputManager.RegistInputMap(this, grid.Get());
    }

    public void OnGridEnd(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if(context.canceled)
            OnGridEndEvent?.Invoke();
    }

    public void OnGridStart(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.started)
            OnGridStartEvent?.Invoke();
    }

    public void OnScreenPosition(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        ScreenPosition = context.ReadValue<Vector3>();
    }
}