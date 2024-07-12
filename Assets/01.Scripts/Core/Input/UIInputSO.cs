using UnityEngine;
using static Controls;

[CreateAssetMenu(menuName = "SO/Input/UIInputSO")]
public class UIInputSO : InputSO, IUIActions
{
    public Vector2 ScreenPosition { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();

        UIActions ui = InputManager.controls.UI;
        ui.SetCallbacks(this);
        InputManager.RegistInputMap(this, ui.Get());
    }
    public void OnScreenPosition(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        ScreenPosition = context.ReadValue<Vector2>();
    }
}
