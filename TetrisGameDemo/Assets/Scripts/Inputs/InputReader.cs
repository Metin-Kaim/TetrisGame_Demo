using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    InputsActions _inputActions;


    private void Start()
    {
        _inputActions = new();
        _inputActions.Movement.Horizontal.Enable();
    }

    public int ReadHorizontalValue()
    {
        return Mathf.RoundToInt(_inputActions.Movement.Horizontal.ReadValue<float>());
    }


}
