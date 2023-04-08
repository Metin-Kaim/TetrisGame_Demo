using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    InputsActions _inputActions;

    public bool IsPressedDown { get; private set; }
    public bool IsPressedRotate { get; private set; }

    private void Start()
    {
        _inputActions = new();
        _inputActions.Movement.Enable();

        _inputActions.Movement.Down.started += PressedToDown;
        _inputActions.Movement.Down.canceled += PressedToDown;

        _inputActions.Movement.Rotate.started += PressedToRotate;
        _inputActions.Movement.Rotate.canceled += PressedToRotate;
    }

    private void PressedToDown(InputAction.CallbackContext obj)
    {
        IsPressedDown = obj.ReadValueAsButton();
    }

    public int ReadHorizontalValue()
    {
        return Mathf.RoundToInt(_inputActions.Movement.Horizontal.ReadValue<float>());
    }

    private void PressedToRotate(InputAction.CallbackContext obj)
    {
        IsPressedRotate = obj.ReadValueAsButton();
    }

}
