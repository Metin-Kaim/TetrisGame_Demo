using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    InputsActions _inputActions;

    public bool IsPressedDown { get; private set; }

    private void Start()
    {
        _inputActions = new();
        _inputActions.Movement.Enable();

        _inputActions.Movement.Down.started += PressedToDown;
        _inputActions.Movement.Down.canceled += UnpressedToDown;
    }

    private void PressedToDown(InputAction.CallbackContext obj)
    {
        IsPressedDown = true;
    }
    private void UnpressedToDown(InputAction.CallbackContext obj)
    {
        IsPressedDown = false;
    }

    public int ReadHorizontalValue()
    {
        return Mathf.RoundToInt(_inputActions.Movement.Horizontal.ReadValue<float>());
    }

}
