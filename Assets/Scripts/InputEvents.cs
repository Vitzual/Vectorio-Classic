using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEvents : MonoBehaviour
{
    public static InputEvents active;

    // Start is called before the first frame update
    public void Awake()
    {
        active = this;
    }

    public event Action onPipettePressed;
    public void PipettePressed()
    {
        if (onPipettePressed != null)
            onPipettePressed();
    }

    public event Action onLeftMouseTapped;
    public void LeftMouseTapped()
    {
        if (onLeftMouseTapped != null)
            onLeftMouseTapped();
    }

    public event Action onLeftMousePressed;
    public void LeftMousePressed()
    {
        if (onLeftMousePressed != null)
            onLeftMousePressed();
    }

    public event Action onLeftMouseReleased;
    public void LeftMouseReleased()
    {
        if (onLeftMouseReleased != null)
            onLeftMouseReleased();
    }

    public event Action onRightMousePressed;
    public void RightMousePressed()
    {
        if (onRightMousePressed != null)
            onRightMousePressed();
    }

    public event Action onRightMouseReleased;
    public void RightMouseReleased()
    {
        if (onRightMouseReleased != null)
            onRightMouseReleased();
    }

    public event Action onMiddleMousePressed;
    public void MiddleMousePressed()
    {
        if (onMiddleMousePressed != null)
            onMiddleMousePressed();
    }

    public event Action onEscapePressed;
    public void EscapePressed()
    {
        if (onEscapePressed != null)
            onEscapePressed();
    }

    public event Action onShiftPressed;
    public void ShiftPressed()
    {
        if (onShiftPressed != null)
            onShiftPressed();
    }

    public event Action onShifReleased;
    public void ShiftReleased()
    {
        if (onShifReleased != null)
            onShifReleased();
    }

    public event Action onLeftControlPressed;
    public void LeftControlPressed()
    {
        if (onLeftControlPressed != null)
            onLeftControlPressed();
    }

    public event Action onLeftControlReleased;
    public void LeftControlReleased()
    {
        if (onLeftControlReleased != null)
            onLeftControlReleased();
    }

    public event Action onPausePressed;
    public void PausePressed()
    {
        if (onPausePressed != null)
            onPausePressed();
    }

    public event Action onInventoryPressed;
    public void InventoryPressed()
    {
        if (onInventoryPressed != null)
            onInventoryPressed();
    }

    public event Action<int> onNumberInput;
    public void NumberInput(int number)
    {
        if (onNumberInput != null)
            onNumberInput(number);
    }
}
